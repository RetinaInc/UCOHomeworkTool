﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UCOHomeworkTool.CustomValidation;

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
        [Display(Name = "Course Name")]
        public string Course { get; set; }
        [Required]
        [Display(Name = "Assignment Number")]
        [Range(1, int.MaxValue, ErrorMessage = "Assignment Number must be positive and not 0")]
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
        [Range(1, int.MaxValue, ErrorMessage = "Problem number must be positive and not 0")]
        [Display(Name="Problem Number")]
        public int ProblemNumber { get; set; }
        public string Description { get; set; }
        public List<EditGivenViewModel> Givens { get; set; }
        public List<EditResponseViewModel> Responses { get; set; }
        public HttpPostedFileBase Diagram { get; set; }
        [Display(Name = "Calculation String")]
        [RequiredWhenCreating(ErrorMessage = "You must enter a value for Calculation String when creating a new problem.")]
        public string CalcString { get; set; }

    }
    public class EditGivenViewModel
    {
        public EditGivenViewModel()
        {
            MinValue = 1;
            MaxValue = 10;
        }
        public int Id { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        [Display(Name = "Minimum Value")]
        [Range(1.0, Double.MaxValue)]
        public double MinValue { get; set; }
        [Required]
        [Display(Name = "Maximum Value")]
        [Range(1.0, Double.MaxValue)]
        public double MaxValue { get; set; }

    }
    public class EditResponseViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
    }
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {

        }
        public EditUserViewModel(ApplicationUser appUser)
        {
            Id = appUser.Id;
            FirstName = appUser.FirstName;
            LastName = appUser.LastName;
            UserName = appUser.UserName;
        }
        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Student ID")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Student or Teacher")]
        public string BaseUserType { get; set; }

        [Display(Name = "Is a Teacher")]
        public bool IsTeacher
        {
            get
            {
                return BaseUserType.Equals("Teacher");
            }
        }
        public bool IsStudent
        {
            get
            {
                return BaseUserType.Equals("Student");
            }
        }
        [Display(Name = "Is an Admin")]
        public bool IsAdmin { get; set; }
    }
}