
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ResumeBuilderProject.ViewModels;
using ResumeBuilderProject.EDMXModel;
using AutoMapper;
using System;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace ResumeBuilderProject.Controllers
{
    public class ResumeController : Controller
    {

        private ProjectResumeEntities _dbContext = new ProjectResumeEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult SavePersonalDetails(EDMXModel.Person person)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Persons.Add(person);
                    _dbContext.SaveChanges();

                    TempData["PersonId"] = person.IDPerson; 
                    return RedirectToAction("Education");
                }
            } catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving personal details.");
            }
            return View("SavePersonalDetails", person);

        }
        public ActionResult Education()
        {
           
            if (TempData.ContainsKey("PersonId") && TempData["PersonId"] is int personId)
            {
                ViewBag.PersonId = personId;
                return View();
            }
            else
            {
                return View();
            }

        }   

        [HttpPost]
        public ActionResult SaveEducation(Education education)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Educations.Add(education);
                _dbContext.SaveChanges();
                return RedirectToAction("WorkExperience");
            }

            ViewBag.PersonId = education.IDPerson;
            return View("Education", education);
        }

        public ActionResult WorkExperience()
        {
            
             if (TempData.ContainsKey("PersonId") && TempData["PersonId"] is int personId)
            {
                ViewBag.PersonId = personId;
                return View();
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult SaveWorkExperience(EDMXModel.WorkExperience workExperience)
        {
            if (ModelState.IsValid)
            {
                _dbContext.WorkExperiences.Add(workExperience);
                _dbContext.SaveChanges();
                return RedirectToAction("Skills");
            }

            ViewBag.PersonId = workExperience.IDPerson;
            return View("WorkExperience", workExperience);
        }

        public ActionResult Skills()
        {
            
            if (TempData.ContainsKey("PersonId") && TempData["PersonId"] is int personId)
            {
                ViewBag.PersonId = personId;
                return View();
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Skill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Skills.Add(skill);
                _dbContext.SaveChanges();
                return RedirectToAction("DownloadResume");
            }
            ViewBag.PersonId = skill.IDPerson;
            return View("Skills", skill);

        }

        public ActionResult DownloadResume()
        {
            int? personId = TempData["PersonId"] as int?;
            if (personId.HasValue)
            {
                var person = _dbContext.Persons.Find(personId.Value);
                if (person != null)
                {
                    var educations = _dbContext.Educations.Where(e => e.IDPerson == personId).ToList();
                    var workExperiences = _dbContext.WorkExperiences.Where(w => w.IDPerson == personId).ToList();
                    var skills = _dbContext.Skills.Where(s => s.IDPerson == personId).ToList();

                    ViewBag.Person = person;
                    ViewBag.Educations = educations;
                    ViewBag.WorkExperiences = workExperiences;
                    ViewBag.Skills = skills;

                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DownloadTextResume()
        {
            int personId = (int)TempData["PersonId"];
            var person = _dbContext.Persons.Find(personId);

            if (person != null)
            {
                var educations = _dbContext.Educations.Where(e => e.IDEdu == personId).ToList();
                var workExperiences = _dbContext.WorkExperiences.Where(w => w.IDExp == personId).ToList();
                var skills = _dbContext.Skills.Where(s => s.IDSki == personId).ToList();

                string resumeText = $"Name: {person.FirstName} {person.LastName}\n" +
                               $"Email: {person.DateOFBirth}\n" +
                               $"Phone: {person.Nationality}\n" +
                               $"Phone: {person.Address}\n" +
                               $"Phone: {person.TellNo}\n" +
                               $"Phone: {person.Email}\n" +
                               "Education:\n";

                foreach (var education in educations)
                {
                    resumeText += $"{education.Degree} in {education.University}\n";
                }

                resumeText += "\nWork Experience:\n";

                foreach (var experience in workExperiences)
                {
                    resumeText += $"{experience.Title} at {experience.Company}\n";
                }

                resumeText += "\nSkills:\n";

                foreach (var skill in skills)
                {
                    resumeText += $"{skill.SkillName}\n";
                }

                var fileName = $"Resume_{person.FirstName}_{person.LastName}.txt";

             
                return File(new System.Text.UTF8Encoding().GetBytes(resumeText), "text/plain", fileName);
            }

            
            
            return RedirectToAction("Index");
           
        }



    }
}