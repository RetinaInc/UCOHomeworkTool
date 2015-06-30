using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCOHomeworkTool.Models
{
    public class EditAssignmentViewModel
    {
        public EditAssignmentViewModel()
        {
            Problems = new List<EditProblemViewModel>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name="Course Name")]
        public string Course{ get; set; }
        [Required]
        [Display(Name="Assignment Number")]
        public int AssignmentNumber { get; set; }
        public List<EditProblemViewModel> Problems { get; set; }

    }
    public class EditProblemViewModel
    {
        public EditProblemViewModel()
        {
            Givens = new List<EditGivenViewModel>();
            Responses = new List<EditResponseViewModel>();
        }
        public int Id { get; set; }
        [Required]
        public int ProblemNumber { get; set; }
        public string Description { get; set; }
        public List<EditGivenViewModel> Givens { get; set; }
        public List<EditResponseViewModel> Responses { get; set; }
        public HttpPostedFileBase Diagram{ get; set; }

    }
    public class EditGivenViewModel
    { 
        public int Id { get; set; }
        [Required]
        public string Label{ get; set; }
        [Required]
        [Display(Name="Minimum Value")]
        public int MinValue { get; set; }
        [Required]
        [Display(Name="Maximum Value")]
        public int MaxValue { get; set; }

    }
    public class EditResponseViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
}