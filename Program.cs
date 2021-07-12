using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Dynamic.Json;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using CommandLine;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsoleApp1
{
    public class Options
    {
        [Option("formId", Default = "97f35010-08de-eb11-bacb-000d3a8fff1f", Required = false, HelpText = "formid")]
        public string FormId { get; set; }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>(o =>
              {
                  var connector = new CDSConnector();
                  var response = connector.QueryFormDefintions(o.FormId).Result;
                  var list = ((IEnumerable)response.value).Cast<dynamic>()
                  .GroupBy(x => x?.form_formdefinitionid?.ToString(), (k, g) =>
                  {
                      var first = g.FirstOrDefault();
                      return new
                      {
                          formDefinitionId = k,
                          form_name = first?.form_name?.ToString(),
                          form_description = first?.form_description?.ToString(),
                          form_rendertype = (int?)first?.form_rendertype,
                          form_order = (int?)first?.form_order,

                          orderedSections = ((IEnumerable)g).Cast<dynamic>()
                          .GroupBy(s => s?.orderedsection_formsectionid?.ToString(), (k, g) =>
                          {
                              var first = g.FirstOrDefault();
                              return new
                              {
                                  formSectionId = k,
                                  name = first?.orderedsection_name?.ToString(),
                                  order = (int?)first?.orderedsection_order,

                                  questions = ((IEnumerable)g).Cast<dynamic>()
                                    .GroupBy(q => q?.question_questionid?.ToString(), (k, g) =>
                                    {
                                        var first = g.FirstOrDefault();
                                        return new
                                        {
                                            question_questionid = k,
                                            formSectionId = first?.question_formsectionid?.ToString(),
                                            name = first?.question_name?.ToString(),
                                            text = first?.question_text?.ToString(),
                                            description = first?.question_description?.ToString(),
                                            type = (int?)first?.question_type,
                                            compoundType = (int?)first?.question_compoundtype,
                                            isCollection = (bool?)first?.question_iscollection,
                                            order = (int?)first?.question_order,
                                            validation_isMandatory = (bool?)first?.question_validation_ismandatory,
                                            validation_options = first?.question_validation_options?.ToString(),
                                            validation_dropdownUrl = first?.question_validation_dropdownurl?.ToString(),
                                            validation_maxLength = (int?)first?.question_validation_text_maxlength,
                                            validation_mask = first?.question_validation_text_mask?.ToString(),
                                            validation_regularExpression = first?.question_validation_text_regularexpression?.ToString(),
                                            validation_minNumber = (int?)first?.question_validation_number_minnumber,
                                            validation_maxNumber = (int?)first?.question_validation_number_maxnumber,
                                            validation_decimalPrecision = (int?) first?.question_validation_number_decimalprecision,
                                            validation_textareaRows = (int?)first?.question_validation_textarea_rows,

                                            compoundQuestions = ((IEnumerable)g).Cast<dynamic>()
                                            .GroupBy(cq => cq?.compoundquestion_questionid?.ToString(), (k, g) =>
                                            {
                                                var first = g.FirstOrDefault();
                                                return new
                                                {
                                                    questionId = k,
                                                    name = first?.compoundquestion_name?.ToString(),
                                                    formSectionId = first?.compoundquestion_formsectionid?.ToString(),
                                                    text = first?.compoundquestion_text?.ToString(),
                                                    description = first?.compoundquestion_description?.ToString(),
                                                    type = (int?)first?.compoundquestion_type,
                                                    compoundType = (int?)first?.compoundquestion_compoundtype,
                                                    isCollection = (bool?)first?.compoundquestion_iscollection,
                                                    order = (int?)first?.compoundquestion_order,
                                                    validation_isMandatory = (bool?)first?.compoundquestion_validation_ismandatory,
                                                    validation_options = first?.compoundquestion_validation_options?.ToString(),
                                                    validation_dropdownUrl = first?.compoundquestion_validation_dropdownurl?.ToString(),
                                                    validation_maxLength = (int?)first?.compoundquestion_validation_text_maxlength,
                                                    validation_mask = first?.compoundquestion_validation_text_mask?.ToString(),
                                                    validation_regularExpression = first?.compoundquestion_validation_text_regularexpression?.ToString(),
                                                    validation_minNumber = (int?)first?.compoundquestion_validation_number_minnumber,
                                                    validation_maxNumber = (int?)first?.compoundquestion_validation_number_maxnumber,
                                                    validation_decimalPrecision = (int?) first?.compoundquestion_validation_number_decimalprecision,
                                                    validation_textareaRows = (int?)first?.compoundquestion_validation_textarea_rows,
                                                };
                                            })
                                        };
                                    })
                              };
                          })
                      };
                  });

                  string jsonString = JsonConvert.SerializeObject(list, new JsonSerializerSettings
                  {
                      Formatting = Formatting.Indented,
                  });
                  Console.WriteLine(jsonString);
              });
        }
    }

    class CrmConfigs
    {
        public string resource { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string tenantId { get; set; }
    }

    public class CDSConnector
    {
        private HttpClient _client;
        public CDSConnector()
        {
            var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: false);
            var configSection = config.Build().GetSection("crmConfigs").Get<CrmConfigs>();

            this._client = GetClient(configSection).Result;
        }

        private async Task<HttpClient> GetClient(CrmConfigs configs)
        {
            var authContext = new AuthenticationContext($"https://login.microsoftonline.com/{configs.tenantId}");
            var credential = new ClientCredential(configs.clientId, configs.clientSecret);
            var result = await authContext.AcquireTokenAsync(configs.resource, credential);
            var authHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            var client = new HttpClient { BaseAddress = new Uri($"{configs.resource}/api/data/v9.2/") };
            client.DefaultRequestHeaders.Authorization = authHeader;
            // client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=OData.Community.Display.V1.FormattedValue");
            return client;
        }

        public async Task<dynamic> QueryFormDefintions(string formId)
        {
            var fetchXml = $@"
<fetch>
  <entity name='form_formdefinition'>
    <attribute name='form_formdefinitionid' />
    <attribute name='form_description' />
    <attribute name='form_name' />
    <attribute name='form_rendertype' />
    <attribute name='form_order' />
    <order attribute='form_order' />
    <link-entity name='form_formorderedsection' from='form_formonwhichorderedsectionappearid' to='form_formdefinitionid' link-type='inner' alias='orderedsection'>
      <attribute name='form_formorderedsectionid' alias='orderedsection_orderedsectionid' />
      <attribute name='form_formonwhichorderedsectionappearid' alias='orderedsection_formdefinitionid' />
      <attribute name='form_sectionthatappearsontheformid' alias='orderedsection_formsectionid' />
      <attribute name='form_name' alias='orderedsection_name' />
      <attribute name='form_order' alias='orderedsection_order' />
      <order attribute='form_order' />
      <link-entity name='form_formsection' from='form_formsectionid' to='form_sectionthatappearsontheformid' alias='section'>
        <attribute name='form_formsectionid' alias='section_formsectionid' />
        <attribute name='form_name' alias='section_name' />
        <attribute name='form_description' alias='section_description' />
      </link-entity>
      <link-entity name='form_question' from='form_formsection' to='form_sectionthatappearsontheformid' alias='question'>
        <attribute name='form_questionid' alias='question_questionid' />
        <attribute name='form_formsection' alias='question_formsectionid' />
        <attribute name='form_name' alias='question_name' />
        <attribute name='form_question_text' alias='question_text' />
        <attribute name='form_description' alias='question_description' />
        <attribute name='form_questiontype' alias='question_type' />
        <attribute name='form_compoundquestiontype' alias='question_compoundtype' />
        <attribute name='form_iscollection' alias='question_iscollection' />
        <attribute name='form_questionorderinsection' alias='question_order' />
        <attribute name='form_mandatory' alias='question_validation_ismandatory' />
        <attribute name='form_options' alias='question_validation_options' />
        <attribute name='form_dropdown_url' alias='question_validation_dropdownurl' />
        <attribute name='form_maxlength' alias='question_validation_text_maxlength' />
        <attribute name='form_mask' alias='question_validation_text_mask' />
        <attribute name='form_regularexpression' alias='question_validation_text_regularexpression' />
        <attribute name='form_minimumnumber' alias='question_validation_number_minnumber' />
        <attribute name='form_maximumnumber' alias='question_validation_number_maxnumber' />
        <attribute name='form_decimalprecision' alias='question_validation_number_decimalprecision' />
        <attribute name='form_textarearows' alias='question_validation_textarea_rows' />
        <order attribute='form_questionorderinsection' />
        <link-entity name='form_questiongrouping' from='form_parentquestionid' to='form_questionid' link-type='outer' alias='questiongrouping'>
          <attribute name='form_questiongroupingid' alias='questiongrouping_questiongroupid' />
          <attribute name='form_parentquestionid' alias='questiongrouping_parentquestionid' />
          <attribute name='form_questionid' alias='questiongrouping_questionid' />
          <attribute name='form_questionorderinorder' alias='questiongrouping_order' />
          <link-entity name='form_question' from='form_questionid' to='form_questionid' link-type='outer' alias='compound'>
            <attribute name='form_questionid' alias='compoundquestion_questionid' />
            <attribute name='form_formsection' alias='compoundquestion_formsectionid' />
            <attribute name='form_name' alias='compoundquestion_name' />
            <attribute name='form_question_text' alias='compoundquestion_text' />
            <attribute name='form_description' alias='compoundquestion_description' />
            <attribute name='form_questiontype' alias='compoundquestion_type' />
            <attribute name='form_questionorderinsection' alias='compoundquestion_order' />
            <attribute name='form_mandatory' alias='compoundquestion_validation_ismandatory' />
            <attribute name='form_options' alias='compoundquestion_validation_options' />
            <attribute name='form_dropdown_url' alias='compoundquestion_validation_dropdownurl' />
            <attribute name='form_maxlength' alias='compoundquestion_validation_text_maxlength' />
            <attribute name='form_mask' alias='compoundquestion_validation_text_mask' />
            <attribute name='form_regularexpression' alias='compoundquestion_validation_text_regularexpression' />
            <attribute name='form_minimumnumber' alias='compoundquestion_validation_number_minnumber' />
            <attribute name='form_maximumnumber' alias='compoundquestion_validation_number_maxnumber' />
            <attribute name='form_decimalprecision' alias='compoundquestion_validation_number_decimalprecision' />
            <attribute name='form_textarearows' alias='compoundquestion_validation_textarea_rows' />
            <order attribute='form_questionorderinsection' />
          </link-entity>
        </link-entity>
      </link-entity>
    </link-entity>
    <filter>
      <condition attribute='form_formdefinitionid' operator='eq' value='{formId}'/>
    </filter>
  </entity>
</fetch>";
            var str = await _client.GetStringAsync($"form_formdefinitions?fetchXml={Uri.EscapeDataString(fetchXml)}");
            return DJson.Parse(str);
        }
        public async Task<dynamic> QueryFormDefintions()
        {
            var str = await _client.GetStringAsync($"form_formdefinitions");
            return DJson.Parse(str);
        }

        public async Task<dynamic> WhoAmI()
        {
            var str = await _client.GetStringAsync("WhoAmI");
            return DJson.Parse(str);
        }
    }
}










