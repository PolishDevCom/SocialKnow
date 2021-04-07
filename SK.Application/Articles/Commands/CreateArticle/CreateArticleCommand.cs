﻿using MediatR;
using SK.Application.Common.Mapping;
using SK.Domain.Entities;
using System;

namespace SK.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommand : IRequest<Guid>, IMapTo<Article>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public CreateArticleCommand() {}
        public CreateArticleCommand(ArticleCreateOrEditDto request)
        {
            Id = request.Id;
            Title = request.Title;
            Image = request.Image;
            Abstract = request.Abstract;
            Content = request.Content;
        }
    }
}