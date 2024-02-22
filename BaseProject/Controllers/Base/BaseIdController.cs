using Marketplace.Responses.Base;
using Marketplace.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers.Base
{
    public class BaseIdController<ViewModel> : ControllerBase
        where ViewModel : BaseIdViewModel, new()
    {
        private readonly IBaseIdService<ViewModel> _service;

        public BaseIdController(IBaseIdService<ViewModel> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult> Get(long id)
        {
            return Ok(await _service.Get(id));
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post(ViewModel request)
        {
            _service.Create(request);
            return Ok();
        }


        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Put(ViewModel request)
        {
            _service.Update(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(long id)
        {
            _service.Delete(id);
            return Ok();
        }

        internal static void CleaningGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
