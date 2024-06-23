using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;

namespace EduSphere.Application.Features.Lesson.Queries;

[Authorize]
public record GetLessonQuery(int Id) : IRequest<LessonDto>;

public class GetLessonQueryHandler : IRequestHandler<GetLessonQuery, LessonDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLessonQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LessonDto> Handle(GetLessonQuery request, CancellationToken cancellationToken)
    {
        var lesson = await _context.Lessons
            .AsNoTracking()
            .ProjectTo<LessonDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (lesson == null)
        {
            throw new NotFoundException(nameof(Lesson), request.Id.ToString());
        }

        return lesson;
    }
}
