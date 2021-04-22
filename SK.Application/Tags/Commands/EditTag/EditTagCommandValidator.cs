﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using SK.Application.Common.Resources.Tags;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.Tags.Commands.EditTag
{
    public class EditTagCommandValidator : AbstractValidator<EditTagCommand>
    {
        private readonly IStringLocalizer<TagsResource> _localizer;
        public EditTagCommandValidator(IStringLocalizer<TagsResource> localizer)
        {
            _localizer = localizer;

            RuleFor(a => a.Title).NotEmpty().WithMessage(_localizer["TagValidatorTitleEmpty"]);
        }
    }
}
