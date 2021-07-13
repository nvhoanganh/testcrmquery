using System.Collections.Generic;

namespace testcrmquery
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class CompoundQuestion
    {
        public string QuestionId { get; set; }
        public string Name { get; set; }
        public string FormSectionId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int? CompoundType { get; set; }
        public bool? IsCollection { get; set; }
        public int? Order { get; set; }
        public bool? ValidationIsMandatory { get; set; }
        public string ValidationOptions { get; set; }
        public string ValidationDropdownUrl { get; set; }
        public int? ValidationMaxLength { get; set; }
        public string ValidationMask { get; set; }
        public string ValidationRegularExpression { get; set; }
        public int? ValidationMinNumber { get; set; }
        public int? ValidationMaxNumber { get; set; }
        public int? ValidationDecimalPrecision { get; set; }
        public int? ValidationTextareaRows { get; set; }
    }

    public class Question
    {
        public string QuestionQuestionid { get; set; }
        public string FormSectionId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int? Type { get; set; }
        public int? CompoundType { get; set; }
        public bool? IsCollection { get; set; }
        public int? Order { get; set; }
        public bool? ValidationIsMandatory { get; set; }
        public string ValidationOptions { get; set; }
        public string ValidationDropdownUrl { get; set; }
        public int? ValidationMaxLength { get; set; }
        public string ValidationMask { get; set; }
        public string ValidationRegularExpression { get; set; }
        public int? ValidationMinNumber { get; set; }
        public int? ValidationMaxNumber { get; set; }
        public int? ValidationDecimalPrecision { get; set; }
        public int? ValidationTextareaRows { get; set; }
        public IEnumerable<CompoundQuestion> CompoundQuestions { get; set; }
    }

    public class OrderedSection
    {
        public string FormSectionId { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public IEnumerable<Question> Questions { get; set; }
    }

    public class FormDefinitionDto
    {
        public string FormDefinitionId { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public int? FormRendertype { get; set; }
        public int? FormOrder { get; set; }
        public IEnumerable<OrderedSection> OrderedSections { get; set; }
    }
}