using EduSphere.Application.Common.Interfaces;

namespace EduSphere.Application.Features.Enrollments.Queries;

public record GetEnrollmentsQuery : IRequest<IEnumerable<EnrollmentDto>>;

public class GetEnrollmentsQueryHandler : IRequestHandler<GetEnrollmentsQuery, IEnumerable<EnrollmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEnrollmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EnrollmentDto>> Handle(GetEnrollmentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Enrollments
            .ProjectTo<EnrollmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
