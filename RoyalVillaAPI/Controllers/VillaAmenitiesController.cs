using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVillaAPI.Data;
using RoyalVillaAPI.Models;
using RoyalVillaDTO;
using System.Collections;

namespace RoyalVillaAPI.Controllers
    {
        [Route("api/villa-amenities")]
        [ApiController]
        //[Authorize(Roles = "Customer, Admin")]    
        public class VillaAmenitiesController : ControllerBase
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public VillaAmenitiesController(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            [HttpGet]
            //[Authorize(Roles = "Admin")]
            [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
            public async Task<ActionResult<ApiResponse<List<VillaAmenitiesDTO>>>> GetVillaAmenities()
            {
                var villas = await _db.VillaAmenities.ToListAsync();
                var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(villas);
                var response = ApiResponse<List<VillaAmenitiesDTO>>.Ok(dtoResponseVillaAmenities, "Villa Amenities retrieved successfully");
                return Ok(response);
            }

            [HttpGet("{id:int}")]
            //[AllowAnonymous]    
            [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
            {
                try
                {
                    if(id <= 0)
                    {
                        return NotFound(ApiResponse<object>.NotFound("VillaAmenities ID must be greater than 0"));                                           
                    }
                    var villaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(v => v.Id == id);
                    if(villaAmenities == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));
                    }
                    return Ok(ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Records retrieved successfully"));                
                }
                catch(Exception ex)
                {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while creating the villa: {ex.Message}");
                return StatusCode(500, errorResponse);
                }
            }        
            [HttpPost]
            [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<VillaAmenitiesCreateDTO>>> CreateVillaAmenities(VillaAmenitiesCreateDTO villaAmenitiesDTO)
            {
                try
                {
                    if(villaAmenitiesDTO == null)
                    {
                        return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities data is required"));
                    }

                    var villaExists = await _db.Villa.FirstOrDefaultAsync(v => v.Id == villaAmenitiesDTO.VillaId);

                    if (villaExists == null)
                    {
                        return Conflict(ApiResponse<object>.Conflict($"Villa with the ID '{villaAmenitiesDTO.VillaId}' does not exist."));
                    }

                    VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(villaAmenitiesDTO);
                    villaAmenities.CreatedDate = DateTime.Now;
                await _db.VillaAmenities.AddAsync(villaAmenities);
                    await _db.SaveChangesAsync(); // Save the changes to the database
                
                    var response = ApiResponse<VillaAmenitiesCreateDTO>.CreatedAt(_mapper.Map<VillaAmenitiesCreateDTO>(villaAmenities), "VillaAmenities created successfully");

                    return CreatedAtAction(nameof(CreateVillaAmenities), new {id=villaAmenities.Id}, response);

                }
                catch(Exception ex)
                {
                    var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while creating the villa: {ex.Message}");
                    return StatusCode(500, errorResponse);
                }
            }        
            [HttpPut("{id:int}")]
            [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO villaAmenitiesDTO)
            {
                try
                {
                    if(villaAmenitiesDTO == null)
                    {
                        return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is required"));
                    }

                    if(id != villaAmenitiesDTO.Id)
                    {
                        return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities ID in URL does not match ID in request body"));
                    }

                    var villaExists = await _db.Villa.FirstOrDefaultAsync(v => v.Id == villaAmenitiesDTO.VillaId);
                    
                    if(villaExists== null)
                    {
                        return Conflict(ApiResponse<object>.Conflict($"Villa with the ID '{villaAmenitiesDTO.VillaId}' does not exist."));
                    }

                    var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(v => v.Id == id);

                    if(existingVillaAmenities == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                    }

                   

                    _mapper.Map(villaAmenitiesDTO, existingVillaAmenities);
                    existingVillaAmenities.UpdatedDate = DateTime.Now;

                    await _db.SaveChangesAsync(); // Save the changes to the database
                    var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenitiesDTO), "VillaAmenities updated successfully");
                    return Ok(response);

                }
                catch(Exception ex)
                {
                    var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while updating the villa amenities: {ex.Message}");
                    return StatusCode(500, errorResponse);
                }
            }        
            [HttpDelete("{id:int}")]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
            [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
            {
                try
                {   
                    var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(v => v.Id == id);

                    if(existingVillaAmenities == null)
                    {
                        return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));
                    }

                    _db.VillaAmenities.Remove(existingVillaAmenities);

                    await _db.SaveChangesAsync(); // Save the changes to the database

                    var response = ApiResponse<object>.NoContent("VillaAmenities deleted successfully");
                    return Ok(response);

                }
                catch(Exception ex)
                {
                    var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while deleting the villa amenities: {ex.Message}");
                    return StatusCode(500, errorResponse);
                }
            }        
        }
    }
