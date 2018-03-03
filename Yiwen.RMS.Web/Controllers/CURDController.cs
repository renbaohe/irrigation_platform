using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yiwen.RMS.Web.Controllers
{
    [Authorize]
    public abstract class CURDController<TEntity, TIDType> : BaseController
        where TEntity : class
    {
        protected abstract IEntityCURD<TEntity> Service
        {
            get;
        }

        #region 首页
        public virtual ActionResult Index()
        {
            return OnCreateIndexView();
        }

        #endregion

        #region 查询
        public virtual ActionResult Find(TIDType id)
        {
            return Json(this.Service.FindByKey(id), JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult Look(TIDType id)
        {
            var item = this.Service.FindByKey(id);
            if (item && item.Data != null)
            {
                return OnCreateEditView(item.Data);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost()]
        public virtual ActionResult List(int pageIndex, int pageSize, [JsonTextModel]SearchCondition condition, [JsonTextModel]OrderCondition order)
        {          
            var res = this.Service.ListPage(condition, order, new PageInfo() { PageIndex = pageIndex, PageSize = pageSize });
            return Json(res);
        }
        #endregion

        #region 增加
        public virtual ActionResult Add()
        {
            return OnCreateAddView();
        }

        [HttpPost()]
        [ValidateInput(false)]
        public virtual ActionResult Add(TEntity item)
        {
            if (this.ModelState.IsValid)
            {
                return Json(this.Service.Add(item));
            }
            else
            {
                return Json(new ResultInfo() { Message = "模型校验失败" });
            }
        }
        #endregion

        #region 编辑
        public virtual ActionResult Edit(TIDType id)
        {
            var item = this.Service.FindByKey(id);
            if (item && item.Data != null)
            {
                return OnCreateEditView(item.Data);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost()]
        [ValidateInput(false)]
        public virtual ActionResult Edit(TEntity item)
        {

            if (this.ModelState.IsValid)
            {
                return Json(this.Service.Update(item));
            }
            else
            {
                return Json(new ResultInfo() { Message = "模型校验失败" });
            }
        }
        #endregion

        #region 删除
        [HttpPost()]
        public virtual ActionResult Delete(TIDType id)
        {
            return Json(this.Service.DeleteByKey(id));
        }
        #endregion

        protected virtual ActionResult OnCreateIndexView()
        {
            return View();
        }
        protected virtual ActionResult OnCreateAddView()
        {
            return View();
        }
        protected virtual ActionResult OnCreateLookView(TEntity entity)
        {
            return View(entity);
        }
        protected virtual ActionResult OnCreateEditView(TEntity entity)
        {
            return View(entity);
        }

    }

    public class JsonTextModelAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return JsonModelBinder.Default;
        }

        private class JsonModelBinder : IModelBinder
        {
            private static JsonModelBinder defaultinstance = new JsonModelBinder();

            public static JsonModelBinder Default
            {
                get { return defaultinstance; }
            }

            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                try
                {
                    var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
                    if (string.IsNullOrEmpty(stringified))
                        return null;
                    return JsonConvert.DeserializeObject(stringified, bindingContext.ModelType);
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                    if (bindingContext.ModelType.IsValueType)
                    {
                        return Activator.CreateInstance(bindingContext.ModelType);
                    }
                    else
                    {
                        return null;
                    }

                }

            }
        }
    }


    public abstract class EasyUICURDController<TEntity, TIDType> : CURDController<TEntity, TIDType>
        where TEntity : class
    {
        [HttpPost()]
        public override ActionResult List(int rows, int page, [JsonTextModel]SearchCondition condition, [JsonTextModel]OrderCondition order)
        {
            OR_condition(ref condition);
            var pageres = this.Service.ListPage(condition, order, new PageInfo() { PageIndex = page - 1, PageSize = rows });
            if (pageres.Success)
            {
                return Json(new { total = pageres.Data.TotalCount, rows = pageres.Data.ListData });
            }
            else
            {
                return Json(null);
            }

        }
        protected override ActionResult OnCreateIndexView()
        {
            return View(this.IndexViewInfo);
        }

        public abstract EasyUIIndexViewInfo IndexViewInfo
        {
            get;
        }

        protected void OR_condition(ref SearchCondition condition)
        {
            if (condition != null)
            {
                if (condition.Items.Count > 0)
                {
                    List<SearchCondition> list = new List<SearchCondition>();
                    foreach (var item in condition.Items)
                    {
                        if (item.Value.ToString() == "-1")
                        {
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            condition.Items.Remove(item);
                        }
                    }
                }
                else
                {
                    if (condition.Value.ToString() == "-1")
                    {
                        condition = null;
                    }
                }
            }
        }
    }


    public class EasyUIIndexViewInfo
    {
        public EasyUIIndexViewInfo()
        {
            this.AddWindowSize = this.EditWindowSize = new Size(400, 500);
            this.AddItemActionName = "Add";
            this.EditItemActionName = "Edit";
            this.DeleteItemActionName = "Delete";
            this.ListItemsActionName = "List";
            this.ShowSearch = true;
            this.DetailItemActionName = "Detail";
        }

        public string DataTitle { get; set; }

        public bool ShowSearch { get; set; }

        public string IDField { get; set; }

        public Size AddWindowSize { get; set; }

        public Size EditWindowSize { get; set; }

        public string ListItemsActionName { get; set; }

        public string AddItemActionName { get; set; }

        public string EditItemActionName { get; set; }

        public string DeleteItemActionName { get; set; }

        public string DetailItemActionName { get; set; }

    }

}
