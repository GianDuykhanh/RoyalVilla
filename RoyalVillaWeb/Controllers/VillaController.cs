using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVillaDTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalVillaWeb.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaList = new();
            try
            {
                var respone = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>("");
                if(respone != null && respone.Success && respone.Data != null)
                {
                    villaList = respone.Data;
                }
            }
            catch (Exception ex) 
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
            }

            return View(villaList);
        }

        public IActionResult Create()
        {
            return View();
        }
        
    }
}