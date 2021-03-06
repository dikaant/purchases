using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Dashboard;
using MixERP.Purchases.ViewModels;

namespace MixERP.Purchases.Controllers.Backend.Tasks
{
    public class EntryController : PurchaseDashboardController
    {
        [Route("dashboard/purchase/tasks/entry/checklist/{tranId}")]
        [MenuPolicy(OverridePath = "/dashboard/purchase/tasks/entry")]
        public ActionResult CheckList(long tranId)
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/CheckList.cshtml", this.Tenant), tranId);
        }

        [Route("dashboard/purchase/tasks/entry")]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/purchase/tasks/entry/new")]
        [MenuPolicy(OverridePath = "/dashboard/purchase/tasks/entry")]
        public ActionResult New()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/New.cshtml", this.Tenant));
        }

        [Route("dashboard/purchase/tasks/entry/new")]
        [HttpPost]
        public async Task<ActionResult> PostAsync(Purchase model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState(this.ModelState);
            }

            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);

            model.UserId = meta.UserId;
            model.OfficeId = meta.OfficeId;
            model.LoginId = meta.LoginId;

            long tranId = await DAL.Backend.Tasks.Purchases.PostAsync(this.Tenant, model).ConfigureAwait(true);
            return this.Ok(tranId);
        }
    }
}