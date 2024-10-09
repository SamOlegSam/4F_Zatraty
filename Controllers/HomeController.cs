using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using ZatratyCore.Models;
using NuGet.Packaging.Signing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Security.Cryptography;
//-Для создания отчетности EXCEL
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.IO;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using OfficeOpenXml;
using FontSize = DocumentFormat.OpenXml.Spreadsheet.FontSize;

namespace ZatratyCore.Controllers
{
    public class md5
    {
        public static string hashPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach (var a in hash)
            {
                sb.Append(a.ToString("X2"));
            }
            return Convert.ToString(sb);
        }
    }


    [Authorize]
    public class HomeController : Controller
    {
        public ZatratyContext db;
        public HomeController(ZatratyContext context)
        {
            db = context;
        }

        //private readonly IWebHostEnvironment _appEnvironment;
        //public HomeController(IWebHostEnvironment appEnvironment)
        //{
        //    _appEnvironment = appEnvironment;
        //}

        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            Autorize UsFil = new Autorize();
            UsFil = db.Autorizes.Include(g=>g.Fillial).FirstOrDefault(k => k.Login == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                        
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult Reference()
        {

            return PartialView();
        }

        public IActionResult LoginPassword()
        {
            List<Autorize> ListUser = new List<Autorize>();
            ListUser = db.Autorizes.Include(u =>u.Fillial).OrderBy(s => s.Login).ToList();
            return View(ListUser);
        }

        //----------Добавление пользователя-----------------------//

        public ActionResult AddLoginPassword()
        {
            List<Fillial> listfil = new List<Fillial>();
            listfil = db.Fillials.ToList();
            ViewBag.FilialLogin = listfil;

            return PartialView();
        }
        //--------------------------//
        //-----Сохранение добавления пользователя-----------//
        [HttpPost]
        public ActionResult AddLoginPasswordSave([FromBody] Autorize mod)
        {
            try
            {
                Autorize aut = new Autorize();
                aut.Login = mod.Login;
                //aut.Password = mod.Password;
                aut.Password = md5.hashPassword(mod.Password);
                aut.FillialId = mod.FillialId;
                aut.Employee = mod.Employee;
                aut.Primechanie = mod.Primechanie;
                aut.Admin = mod.Admin;
                aut.UserMod = mod.UserMod;  
                aut.DateMod = mod.DateMod;
                db.Autorizes.Add(aut);
                db.SaveChanges();

                ViewBag.Message = "Новый пользователь успешно добавлен!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }

            return PartialView();
        }

        // удаление пользователя//

        public ActionResult DeleteLoginPassword(int ID)
        {
            Autorize autDel = new Autorize();
            autDel = db.Autorizes.FirstOrDefault(a => a.Id == ID);
            return PartialView(autDel);
        }
        //-----------------------------//

        // Подтверждение удаления пользователя//
        public ActionResult DeleteLoginPasswordOK(int ID)
        {
            try
            {
                Autorize autDS = new Autorize();
                autDS = db.Autorizes.FirstOrDefault(a => a.Id == ID);
                db.Autorizes.Remove(autDS);
                db.SaveChanges();

                ViewBag.Message = "Пользователь удален!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка удаления";
            }

            return PartialView();
        }
        // Редактирование пользователя//

        public ActionResult LoginPasswordEdit(int ID)
        {
            Autorize autEd = new Autorize();
            autEd = db.Autorizes.FirstOrDefault(a => a.Id == ID);
            
            List<Fillial> listfil = new List<Fillial>();
            listfil = db.Fillials.ToList();
            ViewBag.fillogin = listfil;

            return PartialView(autEd);
        }
        //-------------------------------//

        //Сохранение редактирования пользователя//
        [HttpPost]
        public ActionResult LoginPasswordEditSave([FromBody] Autorize modus)
        {
            try
            {
                var us = HttpContext.User.Identity.Name;
                Autorize autE = new Autorize();
                autE = db.Autorizes.FirstOrDefault(s => s.Id == modus.Id);

                autE.Login = modus.Login;
                //autE.Password = modus.Password;
                autE.Password = md5.hashPassword(modus.Password);
                autE.Employee = modus.Employee;
                autE.FillialId = modus.FillialId;
                autE.Primechanie = modus.Primechanie;
                autE.Admin = modus.Admin;
                autE.UserMod = us;
                autE.DateMod = DateTime.Now;
                db.SaveChanges();

                ViewBag.Message = "Пользователь изменен";

            }
            catch (Exception e)
            {
                ViewBag.Message = "Ошибка. Текст ошибки: " + e.ToString();
            }

            return PartialView();
        }
        //-----------------------------//

        public ActionResult Kateg()
        {
            List<Fillial> Listfilial = new List<Fillial>();
            Listfilial = db.Fillials.OrderBy(s => s.Id).ToList();
            ViewBag.Listfilial = Listfilial;

            List<Role> Listrole = new List<Role>();
            Listrole = db.Roles.Include(y =>y.Plans).OrderBy(s => s.Id).ToList();
            ViewBag.Listrole = Listrole;

            List<Plan> Listplan = new List<Plan>();
            Listplan = db.Plans.OrderBy(s => s.Id).ToList();
            ViewBag.Listplan = Listplan;


            return View(Listrole);
        }

        public ActionResult AddKateg([FromBody] Check  modus)
        {
            try {
                //удалим все записи для записи новых прав
                List<Role> UsRol = new List<Role>();
                UsRol = db.Roles.ToList();

                db.Roles.RemoveRange(UsRol);
                db.SaveChanges();

            List<Role> listrol = new List<Role>();
            
            foreach(var item in modus.check)
            {
               Role role = new Role();
                role.FillialId = item.FillialId;
                role.PlansId = item.PlansId;
                role.UserMod = modus.UserMod;
                role.DateMod = DateTime.Now;
                db.Roles.Add(role);
                db.SaveChanges();
            }
                ViewBag.Message = "Права записаны";

            }
            catch (Exception ex) 
            { 
                ViewBag.Message = ex.ToString(); 
            }                      

            return PartialView();
        }

        public ActionResult PlanKateg()
        {
            List<Plan> Listplan = new List<Plan>();
            Listplan = db.Plans.OrderBy(s => s.Name).ToList();
                        
            return View(Listplan);
        }

        //----------Добавление плана-----------------------//

        public ActionResult AddPlan()
        {
            return PartialView();
        }
        //--------------------------//
        //-----Сохранение добавления плана-----------//
        [HttpPost]
        public ActionResult AddPlanSave([FromBody] Plan mod)
        {
            try
            {
                Plan us = new Plan();
                us.Name = mod.Name;
                us.Primechanie = mod.Primechanie;
                us.UserMod = mod.UserMod;
                us.DateMod = DateTime.Now;
                db.Plans.Add(us);
                db.SaveChanges();

                ViewBag.Message = "Новый вид успешно добавлен!";
            }
            catch (Exception ex)
            {


                ViewBag.Message = ex.ToString();
            }

            return PartialView();
        }

        // удаление плана//

        public ActionResult DeletePlan(int ID)
        {
            Plan planDel = new Plan();
            planDel = db.Plans.FirstOrDefault(a => a.Id == ID);
            return PartialView(planDel);
        }
        //-----------------------------//

        // Подтверждение удаления плана//
        public ActionResult DeletePlanOK(int ID)
        {
            try
            {
                Plan planDS = new Plan();
                planDS = db.Plans.FirstOrDefault(a => a.Id == ID);
                db.Plans.Remove(planDS);
                db.SaveChanges();

                ViewBag.Message = "Вид удален!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Ошибка удаления";
            }

            return PartialView();
        }
        // Редактирование плана//

        public ActionResult PlanEdit(int ID)
        {
            Plan planEd = new Plan();
            planEd = db.Plans.FirstOrDefault(a => a.Id == ID);

            return PartialView(planEd);
        }
        //-------------------------------//

        //Сохранение редактирования вида------------//
        [HttpPost]
        public ActionResult PlanEditSave([FromBody] Plan modus)
        {
            try
            {
                var us = HttpContext.User.Identity.Name;
                Plan planE = new Plan();
                planE = db.Plans.FirstOrDefault(s => s.Id == modus.Id);

                planE.Name = modus.Name;
                planE.Primechanie = modus.Primechanie;
                planE.UserMod = us;
                planE.DateMod = DateTime.Now;
                db.SaveChanges();

                ViewBag.Message = "Вид изменен";

            }
            catch (Exception e)
            {
                ViewBag.Message = "Ошибка. Текст ошибки: " + e.ToString();
            }

            return PartialView();
        }
        //-----------------------------//
        public IActionResult Zatraty()
        {
            List<FillialsExpense> Listzatraty = new List<FillialsExpense>();
            Listzatraty = db.FillialsExpenses.Include(u => u.Fillial).Include(u=>u.TableFillialsExpenses).OrderByDescending(s => s.Date).ToList();
            return View(Listzatraty);
        }

        public IActionResult NewZatraty()
        {
            List<Expense> Listexpenses = new List<Expense>();
            Listexpenses = db.Expenses.OrderBy(s => s.Id).ToList();

                        
            List<ExpenseRoot> listexpenseroot = new List<ExpenseRoot>();
            
            List<Expense> le = new List<Expense>();


            foreach(var i in Listexpenses)
            {
                ExpenseRoot expenseroot = new ExpenseRoot();
                expenseroot.Id = i.Id;
                expenseroot.Name = i.Name;
                if(i.ChildId != null)
                {
                    expenseroot.Parent = Listexpenses.FirstOrDefault(g=>g.Id == i.ChildId);
                }
                else
                {                   
                    expenseroot.Parent = null;
                }

                if (expenseroot.Childs is null)
                {
                    expenseroot.Childs = new List<Expense>();
                }

                if (expenseroot.Childs is null)
                {
                    expenseroot.Childspis = new List<ExpenseRoot>();
                }

                expenseroot.Childs.AddRange(Listexpenses.Where(q=>q.ChildId == i.Id).ToList());
                                                                
                listexpenseroot.Add(expenseroot);
            }
            ViewBag.listexpenseroot = listexpenseroot;

            //-Теперь заполним список с childs

            List<ExpenseRoot> listexpenseroot1 = new List<ExpenseRoot>();

            foreach (var i in Listexpenses)
            {
                ExpenseRoot expenseroot = new ExpenseRoot();
                expenseroot.Id = i.Id;
                expenseroot.Name = i.Name;
                if (i.ChildId != null)
                {   
                    expenseroot.Parent = Listexpenses.FirstOrDefault(g => g.Id == i.ChildId);
                }
                else
                {
                    expenseroot.Parent = null;
                }
                
                if (expenseroot.Childs is null)
                {
                    expenseroot.Childspis = new List<ExpenseRoot>();
                }
                
                if(Listexpenses.Where(t=>t.ChildId == i.Id).Count() != null)
                {
                    foreach(var iw in Listexpenses.Where(t => t.ChildId == i.Id).ToList())
                    {                        
                        expenseroot.Childspis.Add(listexpenseroot.FirstOrDefault(q=>q.Id == iw.Id));
                    }
                }
                
                listexpenseroot1.Add(expenseroot);
            }

               var listexpenseroot1Gr = listexpenseroot1.GroupBy(j => j.Parent);
               ViewBag.listexpenseroot1 = listexpenseroot1;
            //------------------------

            var us = HttpContext.User.Identity.Name;

            var user = db.Autorizes.Include(o=>o.Fillial).FirstOrDefault(t=>t.Login == us);
            ViewBag.user = user;

            //Получим роли для данного филиала
            List<Role> roles = new List<Role>();
            roles = db.Roles.Include(e=>e.Plans).Where(r=>r.FillialId == user.FillialId).ToList();
            ViewBag.roles = roles;
                       

            return View(Listexpenses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ZatratyData()
        {
            var us = HttpContext.User.Identity.Name;

            var user = db.Autorizes.Include(o => o.Fillial).FirstOrDefault(t => t.Login == us);
            ViewBag.userZ = user;

            //Получим роли для данного филиала
            List<Role> roles = new List<Role>();
            roles = db.Roles.Include(e => e.Plans).Where(r => r.FillialId == user.FillialId).ToList();
            ViewBag.rolesZ = roles;

            //Получим список филиалов
            List<Fillial> fil = new List<Fillial>();
            fil = db.Fillials.ToList();
            ViewBag.fil = fil;

            return View();
        }

        public IActionResult TableZatraty([FromBody] Form4F TableZat)
        {
            List<Form4F> list4f = new List<Form4F>();
            list4f = db.Form4Fs.Where(y => y.FilialId == TableZat.FilialId && y.Month == TableZat.Month && y.Year == TableZat.Year).ToList();
            if (list4f.Count > 1)
            {
                var list4fGroup = list4f.GroupBy(t =>t.ExpensesId);
                ViewBag.list4fGroup = list4fGroup;
                ViewBag.list4f = list4f;
            }
            else
            {
                ViewBag.list4fGroup = null;
                ViewBag.list4f = list4f;
            }
            List<Expense> Listexpenses = new List<Expense>();
            Listexpenses = db.Expenses.OrderBy(s => s.Id).ToList();


            List<ExpenseRoot> listexpenseroot = new List<ExpenseRoot>();

            List<Expense> le = new List<Expense>();


            foreach (var i in Listexpenses)
            {
                ExpenseRoot expenseroot = new ExpenseRoot();
                expenseroot.Id = i.Id;
                expenseroot.Name = i.Name;
                if (i.ChildId != null)
                {
                    expenseroot.Parent = Listexpenses.FirstOrDefault(g => g.Id == i.ChildId);
                }
                else
                {
                    expenseroot.Parent = null;
                }

                if (expenseroot.Childs is null)
                {
                    expenseroot.Childs = new List<Expense>();
                }

                if (expenseroot.Childs is null)
                {
                    expenseroot.Childspis = new List<ExpenseRoot>();
                }

                expenseroot.Childs.AddRange(Listexpenses.Where(q => q.ChildId == i.Id).ToList());

                listexpenseroot.Add(expenseroot);
            }
            ViewBag.listexpenseroot = listexpenseroot;

            //-Теперь заполним список с childs

            List<ExpenseRoot> listexpenseroot1 = new List<ExpenseRoot>();

            foreach (var i in Listexpenses)
            {
                ExpenseRoot expenseroot = new ExpenseRoot();
                expenseroot.Id = i.Id;
                expenseroot.Name = i.Name;
                if (i.ChildId != null)
                {
                    expenseroot.Parent = Listexpenses.FirstOrDefault(g => g.Id == i.ChildId);
                }
                else
                {
                    expenseroot.Parent = null;
                }

                if (expenseroot.Childs is null)
                {
                    expenseroot.Childspis = new List<ExpenseRoot>();
                }

                if (Listexpenses.Where(t => t.ChildId == i.Id).Count() != null)
                {
                    foreach (var iw in Listexpenses.Where(t => t.ChildId == i.Id).ToList())
                    {
                        expenseroot.Childspis.Add(listexpenseroot.FirstOrDefault(q => q.Id == iw.Id));
                    }
                }

                listexpenseroot1.Add(expenseroot);
            }

            var listexpenseroot1Gr = listexpenseroot1.GroupBy(j => j.Parent);
            ViewBag.listexpenseroot1 = listexpenseroot1;
            //------------------------

            var us = HttpContext.User.Identity.Name;

            var user = db.Autorizes.Include(o => o.Fillial).FirstOrDefault(t => t.Login == us);
            ViewBag.user = user;

            //Получим роли для данного филиала
            List<Role> roles = new List<Role>();
            //roles = db.Roles.Include(e => e.Plans).Where(r => r.FillialId == user.FillialId).ToList();
            roles = db.Roles.Include(e => e.Plans).Where(r => r.FillialId == TableZat.FilialId).ToList();
            ViewBag.roles = roles;


            return PartialView(Listexpenses);
            
            
       }

        //------------------------------------------------------------------------------//
        [HttpPost]
        public IActionResult TableSave([FromBody] List<Form4F> TableZat)
        {
            //Проверим есть ли записи в БД в соответствии с фильтрами
            List<Form4F> list4f = new List<Form4F>();
            list4f = db.Form4Fs.ToList();
            
            //Найдем запись из переданного списка чтобы определить филиал и дату для заполнения или редактирования данных
            Form4F formFirst = new Form4F();
            formFirst = TableZat.FirstOrDefault();

            //Теперь проверим есть ли запись в БД с данными фильтрами
            List<Form4F> list4fFilter = new List<Form4F>();
            list4fFilter = db.Form4Fs.Where(y => y.FilialId == formFirst.FilialId && y.Month == formFirst.Month && y.Year == formFirst.Year).ToList();
            
            
            if (list4fFilter.Count > 1)
            {
                //Проверим совпадает ли количество записей в таблицей с количеством переданным в данный метод
                if(list4fFilter.Count == TableZat.Count)
                {
                    //Заменим записи в базе записями, переданными в метод                    
                    //на всякий случай обернул редактироване в try/catch
                    try
                    {
                        foreach (var item in list4fFilter)
                        {
                            Form4F f4 = new Form4F();
                            f4 = TableZat.FirstOrDefault(s => (s.ExpensesId == item.ExpensesId && s.PlanId == item.PlanId) || (s.ExpensesId == item.ExpensesId && s.PlanId == Convert.ToInt32(item.Primechanie)));

                            if(f4 !=null)
                            {
                            item.Value = f4.Value;
                            item.UserMod = f4.UserMod;
                            item.DateMod = DateTime.Now;
                            }
                            else
                            {
                                item.Value = null;
                                item.UserMod = HttpContext.User.Identity.Name;
                                item.DateMod = DateTime.Now;
                            }
                            
                            db.SaveChanges();                            

                        }
                           ViewBag.Message = "Данные успешно сохранены";
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "Ошибка. Текст ошибки: " + e.ToString();
                    }
                    
                }
                else
                {
                    ViewBag.Message = "Вы добавили статью затрат и теперь количество записей не совпадает!";
                }
                       return PartialView();
            }
            else
            {
               try
               {
                var us = HttpContext.User.Identity.Name;

                foreach (var item in TableZat)
                {
                Form4F form = new Form4F();
                form.FilialId   = item.FilialId;
                form.Month      = item.Month;
                form.Year       = item.Year;
                form.ExpensesId = item.ExpensesId;
                    if(item.PlanId != 77)
                    {
                        form.PlanId     = item.PlanId;
                    }
                    else
                    {
                        form.Primechanie = item.PlanId.ToString();
                    }
                form.Value      = item.Value;
                form.UserMod    = us;
                form.DateMod = DateTime.Now;

                    db.Form4Fs.Add(form);
                    db.SaveChanges();
                }
               
                ViewBag.Message = "Данные успешно добавлены в БД!";
               }
            catch (Exception ex)
            {


                ViewBag.Message = ex.ToString();
            }
                        
            return PartialView();

            }
            
                    }
        //ФОРМИРОВАНИЕ ОТЧЁТОВ
        public IActionResult ReportZatraty()
        {
            List<Plan> plans = new List<Plan>();
            plans = db.Plans.ToList();
            ViewBag.plans = plans;
            return View();
        }

        public IActionResult ReportFilialy()
        {
            List<Fillial> filial = new List<Fillial>();
            filial = db.Fillials.ToList();
            ViewBag.filialy = filial;

            return View();
        }

        public IActionResult ReportSvod()
        {
            
            return View();
        }

        public IActionResult ReportSumm()
        {
            List<Plan> plans1 = new List<Plan>();
            plans1 = db.Plans.ToList();
            ViewBag.plans1 = plans1;
            return View();
        }


        [HttpPost]
        public IActionResult HtmlZatraty([FromBody] ReportHtml PerZatraty)
        {

            List<Form4F> list4f = new List<Form4F>();
            
                foreach (var item in PerZatraty.Zatraty)
                {
                    List<Form4F> listzatrat = new List<Form4F>();
                    listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >=PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.PlanId == item).ToList();
                    list4f.AddRange(listzatrat);
                }
            
           
            //Сгруппируем полученные данные по видам затрат
            
            var listf4Group = list4f.GroupBy(rt => new {plans = rt.Plan.Name, expensess = rt.Expenses.Name})
                .Select(ty => new { plan = ty.Key.plans, expenses = ty.Key.expensess,  value = ty.Sum(i =>i.Value)})
                .GroupBy(io =>io.plan);
            ViewBag.listufGroup = listf4Group;

            return PartialView();

        }
        
        //---------Печать отчета по затратам в EXCEL-----------------------------------------

        public void ZatratyExcel([FromBody] ReportHtml PerZatraty)
        {
            string filePath1 = "wwwroot/Report/ReportZatratyExcel.xlsx";
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, filePath1);
            CreateExcelFile(filePath);

            void CreateExcelFile(string filePath)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");

                    List<Form4F> list4f = new List<Form4F>();

                    foreach (var item in PerZatraty.Zatraty)
                    {
                        List<Form4F> listzatrat = new List<Form4F>();
                        listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.PlanId == item).ToList();
                        list4f.AddRange(listzatrat);
                    }

                    //Сгруппируем полученные данные по видам затрат

                    var listf4Group = list4f.GroupBy(rt => new { plans = rt.Plan.Name, expensess = rt.Expenses.Name })
                        .Select(ty => new { plan = ty.Key.plans, expenses = ty.Key.expensess, value = ty.Sum(i => i.Value) })
                        .GroupBy(io => io.plan);
                    ViewBag.listufGroup = listf4Group;

                    List<Fillial> listfil = new List<Fillial>();
                    listfil = db.Fillials.ToList();

                    //Заполняем название отчета
                    worksheet.Range("A1:D1").Merge().Value = "Отчёт с разбивкой по видам затрат";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                                            
                    int count = 2;
                    
                    foreach (var i in listf4Group)
                    {
                        count++;
                        worksheet.Cell("B" + count).Value = i.Key;
                        worksheet.Cell("B" + count).Style.Font.Bold = true;
                        worksheet.Cell("B" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        count++;

                        // Заполненяем шапку таблицы 
                        worksheet.Cell("A" + count).Value = "№ п/п";
                        worksheet.Cell("B" + count).Value = "Наименование показателя";
                        worksheet.Cell("C" + count).Value = "Сумма";

                        worksheet.Cell("A" + count).Style.Font.Bold = true;
                        worksheet.Cell("B" + count).Style.Font.Bold = true;
                        worksheet.Cell("C" + count).Style.Font.Bold = true;

                        worksheet.Cell("A" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell("C" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("A" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("C" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        worksheet.Cell("A" + count).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("C" + count).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("C" + count).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("C" + count).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("C" + count).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        worksheet.Cell("B" + count).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        worksheet.Cell("C" + count).Style.Fill.BackgroundColor = XLColor.AliceBlue;

                        count++;
                        int punkt = 1;
                        foreach (var y in i)
                            {
                            worksheet.Cell("A" + count).Value = punkt;
                            worksheet.Cell("B" + count).Value = y.expenses;
                            worksheet.Cell("B" + count).Style.Alignment.WrapText = true;
                            worksheet.Cell("C" + count).Value = y.value;
                            worksheet.Cell("A" + count).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("A" + count).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("A" + count).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("C" + count).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("C" + count).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("C" + count).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            count++;
                            punkt++;
                            }   
                    }                                      
                    
                    //-------------------------------------------------//
                    
                    var col1 = worksheet.Column("A");
                    //col1.AdjustToContents();
                    col1.Width = 5;

                    var col2 = worksheet.Column("B");
                    col2.Width = 50;

                    var col3 = worksheet.Column("C");
                    col3.Width = 15;
                                        
                    // Сохранение файла
                    workbook.SaveAs(filePath);
                }
            }
        }
        //-----------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult HtmlFilialy([FromBody] ReportHtml PerZatraty)
        {

            List<Form4F> list4f = new List<Form4F>();

                        
                foreach (var item in PerZatraty.Zatraty)
                {
                    List<Form4F> listzatrat = new List<Form4F>();
                    listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.FilialId == item).ToList();
                    list4f.AddRange(listzatrat);
                }
                      
            
            var listf4Group = list4f
                .GroupBy(q => new { filials = q.Filial.Filial })
                .Select(o => new { 
                    filialse = o.Key.filials, 
                    plansp = o.GroupBy(uu=>uu.Plan.Name).Select(yy=> new { planspis = yy.Key }),
                    list = o.ToList().GroupBy(io =>io.Expenses.Name)
                    .Select(h=> new {
                        expense = h.Key, spis = h.ToList()
                            .GroupBy(kl=> kl.Plan.Name)
                            .Select(y=> new { 
                                plan = y.Key, value = y.Sum(o=>o.Value)})})})                                
                ;
            
            ViewBag.listufGroupFil = listf4Group;

            return PartialView();

        }

        //---------Печать отчета по филиалам в EXCEL-----------------------------------------

        public void FilialyExcel([FromBody] ReportHtml PerZatraty)
        {
            string filePath1 = "wwwroot/Report/ReportFilialyExcel.xlsx";
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, filePath1);
            CreateExcelFile(filePath);

            void CreateExcelFile(string filePath)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");

                    List<Form4F> list4f = new List<Form4F>();


                    foreach (var item in PerZatraty.Zatraty)
                    {
                        List<Form4F> listzatrat = new List<Form4F>();
                        listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.FilialId == item).ToList();
                        list4f.AddRange(listzatrat);
                    }

                    var listf4Group = list4f
                        .GroupBy(q => new { filials = q.Filial.Filial })
                        .Select(o => new {
                            filialse = o.Key.filials,
                            plansp = o.GroupBy(uu => uu.Plan.Name).Select(yy => new { planspis = yy.Key }),
                            list = o.ToList().GroupBy(io => io.Expenses.Name)
                            .Select(h => new {
                                expense = h.Key,
                                spis = h.ToList()
                                    .GroupBy(kl => kl.Plan.Name)
                                    .Select(y => new {
                                        plan = y.Key,
                                        value = y.Sum(o => o.Value)
                                    })
                            })
                        });

                    //Заполняем название отчета
                    worksheet.Range("A1:D1").Merge().Value = "Отчёт с разбивкой по филиалам";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    int count = 2;

                    foreach (var i in listf4Group)
                    {
                        count++;
                        worksheet.Cell("B" + count).Value = i.filialse;
                        worksheet.Cell("B" + count).Style.Font.Bold = true;
                        worksheet.Cell("B" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        count++;

                        // Заполненяем шапку таблицы 
                        worksheet.Cell("A" + count).Value = "№ п/п";
                        worksheet.Cell("B" + count).Value = "Наименование показателя";

                        int k = 3;
                        foreach (var iu in i.plansp)
                        {
                            worksheet.Cell(count, k).Value = iu.planspis;
                            worksheet.Cell(count, k).Style.Font.Bold = true;
                            worksheet.Cell(count, k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell(count, k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            worksheet.Cell(count, k).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                            worksheet.Cell(count, k).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                            worksheet.Cell(count, k).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                            worksheet.Cell(count, k).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                            worksheet.Cell(count, k).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                            worksheet.Cell(count, k).Style.Alignment.WrapText = true;
                            k++;
                        }
                        worksheet.Cell(count, k).Value = "ИТОГО";
                        worksheet.Cell(count, k).Style.Font.Bold = true;
                        worksheet.Cell(count, k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(count, k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell(count, k).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        worksheet.Cell(count, k).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(count, k).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(count, k).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(count, k).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(count, k).Style.Alignment.WrapText = true;



                        worksheet.Cell("A" + count).Style.Font.Bold = true;
                        worksheet.Cell("B" + count).Style.Font.Bold = true;
                        
                        worksheet.Cell("A" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell("A" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell("B" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        
                        worksheet.Cell("A" + count).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("B" + count).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell("A" + count).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        worksheet.Cell("B" + count).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        
                        count++;
                        int punkt = 1;
                        foreach (var y in i.list)
                        {
                            worksheet.Cell("A" + count).Value = punkt;
                            worksheet.Cell("B" + count).Value = y.expense;
                            worksheet.Cell("B" + count).Style.Alignment.WrapText = true;                                                        
                            worksheet.Cell("A" + count).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("A" + count).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("A" + count).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell("B" + count).Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                            int p = 3;
                            decimal? itogo = 0;
                            foreach(var t in y.spis)
                            {
                            worksheet.Cell(count, p).Value = t.value;                               
                            worksheet.Cell(count, p).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(count, p).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(count, p).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                                p++;
                                itogo = itogo + t.value;
                                worksheet.Cell(count, p).Value = itogo;
                                worksheet.Cell(count, p).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(count, p).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                                worksheet.Cell(count, p).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            }                            
                            count++;
                            punkt++;
                        }
                    }

                    //-------------------------------------------------//

                    var col1 = worksheet.Column("A");
                    //col1.AdjustToContents();
                    col1.Width = 5;

                    var col2 = worksheet.Column("B");
                    col2.Width = 50;

                    var col3 = worksheet.Column("C");
                    col3.Width = 15;

                    // Сохранение файла
                    workbook.SaveAs(filePath);
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public IActionResult HtmlSvod([FromBody] ReportHtml PerZatraty)
        {

            List<Form4F> list4f = new List<Form4F>();

           
                List<Form4F> listzatrat = new List<Form4F>();

            //List<Form4F> listzatrat1 = new List<Form4F>();
            //listzatrat1 = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.Primechanie == "77" ).ToList();

            listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null).ToList();
            list4f.AddRange(listzatrat);
           
            //сгруппируем по видам затрат           
            
            var listf4PrimGroup = listzatrat
                .GroupBy(k => k.Expenses)
                .Select(grt => new { name = grt.Key.Name, id = grt.Key.Id, spis = grt.Select(p => p), summa = grt.Sum(o => o.Value), spisneft = grt.Where(grt =>grt.PlanId != 22).Sum(w =>w.Value) })
                .Select(u => new { Name = u.name, Id = u.id, Summa = u.summa, ValueNeft = u.spisneft /*ValueNeft = u.spis.FirstOrDefault(y=>y.PlanId == 22) != null ? u.spis.FirstOrDefault(y => y.PlanId == 22).Value : 0*/ })
                .Select(pol => new { Name = pol.Name, Id = pol.Id, Summa = pol.Summa, Summa4F = pol.ValueNeft /*Summa4F = pol.ValueNeft != null ? pol.Summa - pol.ValueNeft : pol.Summa*/ }); 
            
            ViewBag.listf4Prim = listf4PrimGroup;
                        
            List<Fillial> listfil = new List<Fillial>();
            listfil = db.Fillials.ToList();
            ViewBag.listfil = listfil;

            //var ld = ListDog.GroupBy(h => h.Sanatorium).Select(grt => new { Name = grt.Key.Name, id = grt.Key.Id, spis = grt.Select(p => p) }).OrderBy(h => h.Name);

            return PartialView();

        }
        //---------Печать сводного отчета в EXCEL-----------------------------------------
        
        public void SvodExcel([FromBody] ReportHtml PerZatraty)
        {            
            string filePath1 = "wwwroot/Report/ReportSvodExcel.xlsx";
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, filePath1);
            //string filePath = "C:/Install/Report.xlsx";
            CreateExcelFile(filePath);

             void CreateExcelFile(string filePath)
            {

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    List<Form4F> list4f = new List<Form4F>();
                    List<Form4F> listzatrat = new List<Form4F>();

                    listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null).ToList();
                    list4f.AddRange(listzatrat);

                    //сгруппируем по видам затрат           

                    var listf4PrimGroup = listzatrat
                        .GroupBy(k => k.Expenses)
                        .Select(grt => new { name = grt.Key.Name, id = grt.Key.Id, spis = grt.Select(p => p), summa = grt.Sum(o => o.Value), spisneft = grt.Where(grt => grt.PlanId != 22).Sum(w => w.Value) })
                        .Select(u => new { Name = u.name, Id = u.id, Summa = u.summa, ValueNeft = u.spisneft /*ValueNeft = u.spis.FirstOrDefault(y=>y.PlanId == 22) != null ? u.spis.FirstOrDefault(y => y.PlanId == 22).Value : 0*/ })
                        .Select(pol => new { Name = pol.Name, Id = pol.Id, Summa = pol.Summa, Summa4F = pol.ValueNeft /*Summa4F = pol.ValueNeft != null ? pol.Summa - pol.ValueNeft : pol.Summa*/ });

                    List<Fillial> listfil = new List<Fillial>();
                    listfil = db.Fillials.ToList();

                    //Заполняем название отчета
                    worksheet.Range("A1:D1").Merge().Value = "Сводный отчет 12-Ф Прибыль 4-Ф Затраты";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                   
                    // Заполненяем шапку таблицы 
                    worksheet.Cell("A3").Value = "№ п/п";
                    worksheet.Cell("B3").Value = "Наименование показателя";
                    worksheet.Cell("C3").Value = "12-Ф Прибыль";
                    worksheet.Cell("D3").Value = "4-Ф Затраты";

                    worksheet.Cell("A3").Style.Font.Bold = true;
                    worksheet.Cell("B3").Style.Font.Bold = true;
                    worksheet.Cell("C3").Style.Font.Bold = true;
                    worksheet.Cell("D3").Style.Font.Bold = true;

                    worksheet.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell("B3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("B3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell("C3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("C3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell("D3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("D3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    int count = 3;
                    foreach (var i in listf4PrimGroup)
                    {
                        count++;
                        if(i.Id == 1)
                        { 
                        worksheet.Cell("A" + count).Value = count-3;
                        worksheet.Cell("B" + count).Value = i.Name;
                        worksheet.Cell("B" + count).Style.Alignment.WrapText = true;
                        worksheet.Cell("C" + count).Value = "X";
                        worksheet.Cell("C" + count).Style.Font.Bold = true;
                        worksheet.Cell("C" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell("C" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell("D" + count).Value = i.Summa4F;
                        }
                        else if(i.Id == 2 || i.Id == 3 || i.Id == 4 || i.Id == 5 || i.Id == 34)
                        {
                            worksheet.Cell("A" + count).Value = count - 3;
                            worksheet.Cell("B" + count).Value = i.Name;
                            worksheet.Cell("B" + count).Style.Alignment.WrapText = true;
                            worksheet.Cell("C" + count).Value = i.Summa;
                            worksheet.Cell("D" + count).Value = "X";
                            worksheet.Cell("D" + count).Style.Font.Bold = true;
                            worksheet.Cell("D" + count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cell("D" + count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        }
                        else
                        {
                            worksheet.Cell("A" + count).Value = count - 3;
                            worksheet.Cell("B" + count).Value = i.Name;
                            worksheet.Cell("B" + count).Style.Alignment.WrapText = true;
                            worksheet.Cell("C" + count).Value = i.Summa;
                            worksheet.Cell("D" + count).Value = i.Summa4F;
                        }
                    }
                    //пример изменения стиля ячейки
                    worksheet.Cell("A" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("B" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("C" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("D" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    
                    var rngTable = worksheet.Range("A3:D" + (listf4PrimGroup.Count() + 3));
                    rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                    //-------------------------------------------------//
                    var rngTable111 = worksheet.Range("A3:d3");
                    rngTable111.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.TopBorder = XLBorderStyleValues.Medium;

                    var col1 = worksheet.Column("A");
                    //col1.AdjustToContents();
                    col1.Width = 5;

                    var col2 = worksheet.Column("B");
                    col2.Width = 50;

                    var col3 = worksheet.Column("C");
                    col3.Width = 15;

                    var col4 = worksheet.Column("D");
                    col4.Width = 15;

                    

                    // Сохранение файла
                    workbook.SaveAs(filePath);
                }
            }
        }
        //-----------------------------------------------------
        [HttpPost]
        public IActionResult HtmlSumm([FromBody] ReportHtml PerZatraty)
        {

            //-Получим список затрат для шапки таблицы----------------------------------------
            List<Plan> spiszatr = new List<Plan>();

            List<Plan> listplans = new List<Plan>();
            listplans = db.Plans.ToList();
            foreach(var i in PerZatraty.Zatraty)
            {
                Plan plan = new Plan();
                plan = listplans.FirstOrDefault(h => h.Id == i);
                spiszatr.Add(plan);
            }
            ViewBag.spiszatr = spiszatr;
            //--------------------------------------------------------------------------------

            List<Form4F> list4f = new List<Form4F>();

            foreach (var item in PerZatraty.Zatraty)
            {
                List<Form4F> listzatrat = new List<Form4F>();
                listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.PlanId == item).ToList();
                list4f.AddRange(listzatrat);
            }

            //Сгруппируем полученные данные по видам затрат

            var listf4Group = list4f
                .GroupBy(rt => rt.Expenses.Name)
                .Select(t => new
                {
                    name = t.Key,
                    spis = t.Select(p => p)
                .GroupBy(i => i.Plan.Name)
                .Select(h => new { plan = h.Key, value = h.Sum(k => k.Value) })
                });
            ViewBag.listufGroup = listf4Group;

            return PartialView();

        }

        //---------Печать суммарного отчета в EXCEL-----------------------------------------

        public void SummExcel([FromBody] ReportHtml PerZatraty)
        {
            string filePath1 = "wwwroot/Report/ReportSummExcel.xlsx";
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, filePath1);
            
            CreateExcelFile(filePath);

            void CreateExcelFile(string filePath)
            {

                using (var workbook = new XLWorkbook())
                {
                    List<Plan> spiszatr = new List<Plan>();

                    List<Plan> listplans = new List<Plan>();
                    listplans = db.Plans.ToList();
                    foreach (var i in PerZatraty.Zatraty)
                    {
                        Plan plan = new Plan();
                        plan = listplans.FirstOrDefault(h => h.Id == i);
                        spiszatr.Add(plan);
                    }

                    var worksheet = workbook.Worksheets.Add("Sheet1");
                    List<Form4F> list4f = new List<Form4F>();
                   
                    foreach (var item in PerZatraty.Zatraty)
                    {
                        List<Form4F> listzatrat = new List<Form4F>();
                        listzatrat = db.Form4Fs.Include(r => r.Filial).Include(o => o.Expenses).Include(q => q.Plan).Where(u => u.Year == PerZatraty.Year && (u.Month >= PerZatraty.S && u.Month <= PerZatraty.Po) && u.PlanId != null && u.PlanId == item).ToList();
                        list4f.AddRange(listzatrat);
                    }

                    //Сгруппируем полученные данные по видам затрат

                    var listf4Group = list4f
                        .GroupBy(rt => rt.Expenses.Name)
                        .Select(t => new
                        {
                            name = t.Key,
                            spis = t.Select(p => p)
                        .GroupBy(i => i.Plan.Name)
                        .Select(h => new { plan = h.Key, value = h.Sum(k => k.Value) })
                        });

                    List<Fillial> listfil = new List<Fillial>();
                    listfil = db.Fillials.ToList();

                    //Заполняем название отчета
                    worksheet.Range("A1:D1").Merge().Value = "Суммарный отчет";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    
                    // Заполненяем шапку таблицы 
                    worksheet.Cell("A3").Value = "№ п/п";
                    worksheet.Cell("B3").Value = "Наименование показателя";
                    
                    worksheet.Cell("A3").Style.Font.Bold = true;
                    worksheet.Cell("B3").Style.Font.Bold = true;
                    
                    worksheet.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell("B3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("B3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    
                    int k = 3;
                    foreach (var iu in spiszatr)
                    {                        
                        worksheet.Cell(3, k).Value = iu.Name;                        
                        worksheet.Cell(3, k).Style.Font.Bold = true;
                        worksheet.Cell(3, k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(3, k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        worksheet.Cell(3, k).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        worksheet.Cell(3, k).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(3, k).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(3, k).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(3, k).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Cell(3, k).Style.Alignment.WrapText = true;
                        k++;
                    }
                    worksheet.Cell(3, k).Value = "ИТОГО";
                    worksheet.Cell(3, k).Style.Font.Bold = true;
                    worksheet.Cell(3, k).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(3, k).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(3, k).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell(3, k).Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(3, k).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(3, k).Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(3, k).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell(3, k).Style.Alignment.WrapText = true;


                    int count = 3;
                    foreach (var i in listf4Group)
                    {
                        count++;
                        decimal? itog = 0;
                        int kk = 3;
                        worksheet.Cell(count, 1).Value = count - 3;
                        worksheet.Cell(count, 2).Value = i.name;
                        worksheet.Cell(count,2).Style.Alignment.WrapText = true;
                        worksheet.Cell(count, 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(count, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(count, 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        foreach (var j in i.spis)
                        { 
                        worksheet.Cell(count, kk).Value = j.value;
                            worksheet.Cell(count, kk).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(count, kk).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            worksheet.Cell(count, kk).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            kk++;
                            itog = itog + j.value;
                    }
                        worksheet.Cell(count, kk).Value = itog;
                        worksheet.Cell(count, kk).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(count, kk).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(count, kk).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    }
                    //пример изменения стиля ячейки
                    worksheet.Cell("A" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("B" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("C" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;
                    worksheet.Cell("D" + 3).Style.Fill.BackgroundColor = XLColor.AliceBlue;

                    var rngTable = worksheet.Range("A3:D" + (listf4Group.Count() + 3));
                    rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.LeftBorder = XLBorderStyleValues.Thin;

                    //-------------------------------------------------//
                    var rngTable111 = worksheet.Range("A3:d3");
                    rngTable111.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    rngTable111.Style.Border.TopBorder = XLBorderStyleValues.Medium;

                    var col1 = worksheet.Column("A");
                    //col1.AdjustToContents();
                    col1.Width = 5;

                    var col2 = worksheet.Column("B");
                    col2.Width = 50;

                    var col3 = worksheet.Column("C");
                    col3.Width = 15;

                    var col4 = worksheet.Column("D");
                    col4.Width = 15;



                    // Сохранение файла
                    workbook.SaveAs(filePath);
                }
            }
        }

        //-----------------------------------------------------
    }
}
