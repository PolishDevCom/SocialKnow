using MediatR;

namespace SK.Application.Photos.Commands.SetMainPhoto
{
    public class SetMainPhotoCommand : IRequest
    {
        public SetMainPhotoCommand() {}

        public SetMainPhotoCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
