﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Classes;

namespace ECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.City).Include(c => c.Department);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartment(), "DepartmentId", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {

                db.Companies.Add(company);

                try
                {
                    db.SaveChanges();



                    if (company.LogoFile != null)
                    {
                        var picture = string.Empty;
                        var folder = "~/Content/Logos";

                        var response =  FileHelper.UploadPhoto(company.LogoFile, folder, $"{company.CompanyId}.jpg");

                        if (response)
                        {
                            picture = $"{folder}/{company.CompanyId}.jpg";

                            company.Logo = picture;

                            //actualizo la db:
                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }


                    }

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Value.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }



            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartment(), "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Company company = db.Companies.Find(id);

            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartment(), "DepartmentId", "Name", company.DepartmentId);

            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {

                if (company.LogoFile != null)
                {
                    var picture = string.Empty;
                    var folder = "~/Content/Logos";

                    var response = FileHelper.UploadPhoto(company.LogoFile, folder, $"{company.CompanyId}.jpg");

                    if (response)
                    {
                        picture = $"{folder}/{company.CompanyId}.jpg";

                        company.Logo = picture;

                        //actualizo la db:
                        //db.Entry(company).State = EntityState.Modified;
                        //db.SaveChanges();
                    }


                }


                db.Entry(company).State = EntityState.Modified;

                try
                {

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Value.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }


            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartment(), "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);

            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "The Record can't be Delete because it has related  Records");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View(company);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
