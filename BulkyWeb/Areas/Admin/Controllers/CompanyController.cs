using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
 
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
              
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }            
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                
                if(companyObj.Id==0) 
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Comapny Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }


       
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objProductList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data= objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyTOBeDeleted= _unitOfWork.Company.Get(u=>u.Id==id);
            if(CompanyTOBeDeleted == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }
            
            _unitOfWork.Company.Remove(CompanyTOBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message="Delete Successful" }) ;
        }
        #endregion

    }
}
