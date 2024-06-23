using EduSphere.Application.Common.Interfaces;
using EduSphere.Domain.Entities;

namespace EduSphere.Application.Features.Enrollments.Queries;

public record GetEnrollmentQuery(int Id) : IRequest<EnrollmentDto>;

public class GetEnrollmentQueryHandler : IRequestHandler<GetEnrollmentQuery, EnrollmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEnrollmentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EnrollmentDto> Handle(GetEnrollmentQuery request, CancellationToken cancellationToken)
    {
        var enrollment = await _context.Enrollments
            .AsNoTracking()
            .ProjectTo<EnrollmentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (enrollment == null)
        {
            throw new NotFoundException(nameof(Enrollment), request.Id.ToString());
        }

        return enrollment;
    }
}
