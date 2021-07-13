using System.Collections.Generic;
using Newtonsoft.Json;

namespace testcrmquery
{
    public class Root
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Value
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }
        public int form_order { get; set; }
        public string form_name { get; set; }
        public int form_rendertype { get; set; }
        public string form_formdefinitionid { get; set; }
        public string form_description { get; set; }
        public string compoundquestion_formsectionid { get; set; }
        public bool question_validation_ismandatory { get; set; }
        public string compoundquestion_name { get; set; }
        public string orderedsection_formsectionid { get; set; }
        public string questiongrouping_parentquestionid { get; set; }
        public string compoundquestion_questionid { get; set; }
        public string orderedsection_orderedsectionid { get; set; }
        public string orderedsection_name { get; set; }
        public string section_formsectionid { get; set; }
        public string question_text { get; set; }
        public bool compoundquestion_validation_ismandatory { get; set; }
        public string question_formsectionid { get; set; }
        public int compoundquestion_type { get; set; }
        public int question_compoundtype { get; set; }
        public string compoundquestion_text { get; set; }
        public string questiongrouping_questiongroupid { get; set; }
        public int compoundquestion_order { get; set; }
        public string questiongrouping_questionid { get; set; }
        public string section_name { get; set; }
        public string orderedsection_formdefinitionid { get; set; }
        public bool question_iscollection { get; set; }
        public string question_name { get; set; }
        public int questiongrouping_order { get; set; }
        public int compoundquestion_validation_text_maxlength { get; set; }
        public int orderedsection_order { get; set; }
        public string question_questionid { get; set; }
        public int question_type { get; set; }
        public string question_description { get; set; }
        public string compoundquestion_description { get; set; }
        public string question_validation_options { get; set; }
        public string question_validation_dropdownurl { get; set; }
        public string compoundquestion_validation_options { get; set; }
        public string compoundquestion_validation_dropdownurl { get; set; }
        public string compoundquestion_validation_text_mask { get; set; }
        public string compoundquestion_validation_text_regularexpression { get; set; }
        public int question_validation_number_minnumber { get; set; }
        public int question_validation_number_maxnumber { get; set; }
        public int question_validation_number_decimalprecision { get; set; }
        public int compoundquestion_validation_number_minnumber { get; set; }
        public int compoundquestion_validation_number_maxnumber { get; set; }
        public int compoundquestion_validation_number_decimalprecision { get; set; }
        public int compoundquestion_validation_textarea_rows { get; set; }
        public int question_validation_textarea_rows { get; set; }
        public int compoundquestion_compoundtype { get; set; }
        public bool compoundquestion_iscollection { get; set; }
        public int? question_order { get; set; }
        public int? question_validation_text_maxlength { get; set; }
        public string question_validation_text_regularexpression { get; set; }
        public string question_validation_text_mask { get; set; }
    }

    public class FormDto
    {
        
    }
}