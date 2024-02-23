using Marketplace.Responses.Base;
using Marketplace.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers.Base
{
    public class BaseGuidController<ViewModel> : ControllerBase
        where ViewModel : BaseGuidViewModel, new()
    {
        private readonly IBaseGuidService<ViewModel> _service;

        public BaseGuidController(IBaseGuidService<ViewModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult> GetAll()
        {
            return Ok(await _service.All());
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> Get(Guid id)
        {
            return Ok(await _service.GetById(id));
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post(ViewModel request)
        {
            _service.Add(request);
            return Ok();
        }


        //[HttpPut("{id}")]
        //public virtual async Task<ActionResult> Put(ViewModel request)
        //{
        //    _service.Update(request);
        //    return Ok();
        //}

        //[HttpDelete("{id}")]
        //public virtual async Task<ActionResult> Delete(Guid id)
        //{
        //    _service.Delete(id);
        //    return Ok();
        //}

        internal static void CleaningGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
