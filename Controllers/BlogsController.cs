﻿using Blog.Api.DtoModels;
using Blog.Api.Managers;
using Blog.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BlogsController : ControllerBase
{
    private readonly BlogManager _manager;
    private readonly PostManager _postManager;

    public BlogsController(BlogManager manager, PostManager postManager)
    {
        _manager = manager;
        _postManager = postManager;
    }

    [HttpGet]
    public async Task<List<BlogModel>> GetAllTheBlogs()
    {
        return await _manager.GetAllTheBlogs();
    }

    [HttpPost]
    public async Task<BlogModel> CreateBlog(BlogDto model)
    {
        var blog = await _manager.CreateBlog(model);
        return  blog;
    }

    [HttpGet("GetById/{Id}")]
    public async Task<BlogModel> GetBlogById(Guid Id)
    {
        var blog = await _manager.GetBlogById(Id);
        return blog;
    }
    [HttpGet("Delete/{Id}")]
    public async Task DeleteBlog(Guid Id)
    {
        await _manager.DeleteBlog(Id);

    }

    [HttpPost("{blogId}/post")]
    public async Task<PostModel> CreatePost([FromForm]PostDto dto,Guid blogId)
    {
        var post = await _postManager.CreatePost(blogId, dto);
        return post;
    }

    [HttpGet("{blogId}/posts")]
    public async Task<List<PostModel>> AllthePosts(Guid blogId)
    {
        var list = await _postManager.AllPosts(blogId);
        return list;
    }
    [HttpGet("{blogId}/posts/{postId}")]
    public async Task<PostModel> GetPostById(Guid blogId,Guid postId)
    {
        var post = await _postManager.GetPostById(blogId, postId);
        return post;
    }





    /*[HttpGet("Update")]
    public async Task<Entities.Blog?> UpdateBlog(Entities.Blog blog)
    {
        var blogUpdated = await _manager.UpdateBlog(blog);
        return blogUpdated;
    }*/
}