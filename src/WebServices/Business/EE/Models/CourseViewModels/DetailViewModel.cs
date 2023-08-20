﻿using System.ComponentModel.DataAnnotations;

namespace Aiursoft.EE.Models.CourseViewModels;

public class DetailViewModel
{
    public string Name { get; set; }
    public string TeacherDescription { get; set; }
    public string Description { get; set; }

    [Display(Name = "What you will learn")]
    public string WhatYouWillLearn { get; set; }

    public bool Subscribed { get; set; }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public bool IsOwner { get; set; }
    public bool DisplayOwnerInfo { get; set; }

    public IEnumerable<Section> Sections { get; set; }
    public List<Chapter> Chapters { get; set; }
}