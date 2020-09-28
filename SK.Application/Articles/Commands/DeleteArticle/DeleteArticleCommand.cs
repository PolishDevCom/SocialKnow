﻿using MediatR;
using System;

namespace SK.Application.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
