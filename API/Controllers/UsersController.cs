using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRespository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRespository, IMapper mapper)
        {
            _mapper = mapper;
            _userRespository = userRespository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRespository.GetMembersAsync();
            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRespository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
          var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var user = await _userRespository.GetUserByUserNameAsync(username);
          _mapper.Map(memberUpdateDto, user);
          _userRespository.Update(user);

          if (await _userRespository.SaveAllAsync()) return NoContent();
          
          return BadRequest("Failed to update user");
        }

    }

}




