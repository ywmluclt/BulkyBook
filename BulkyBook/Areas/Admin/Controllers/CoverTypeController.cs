using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
            {
                //Create new CoverType
                return View(coverType);
            }
            // Edit CoverType
            //coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@Id", id);
            coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, dynamicParameter);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var dynamicParameter = new DynamicParameters();
                dynamicParameter.Add("@Name", coverType.Name);

                if (coverType.Id == 0)
                {
                    //_unitOfWork.CoverType.Add(coverType);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, dynamicParameter);
                }
                else
                {
                    dynamicParameter.Add("@Id", coverType.Id);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, dynamicParameter);
                    //_unitOfWork.CoverType.Update(coverType);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(coverType);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            //var allCatelogy = _unitOfWork.CoverType.GetAll();
            var allCatelogy = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);

            return Json(new { data = allCatelogy });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var coverTypeFromDb = _unitOfWork.CoverType.Get(id);
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@Id", id);
            var coverTypeFromDb = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, dynamicParameter);

            if (coverTypeFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //_unitOfWork.CoverType.Remove(coverTypeFromDb);
            _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, dynamicParameter);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
