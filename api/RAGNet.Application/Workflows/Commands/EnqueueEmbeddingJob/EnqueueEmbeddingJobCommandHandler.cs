using RAGNET.Application.Configuration.Commands;
using RAGNET.Application.Workflows.CallbackUrls.Mappers;
using RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob.Jobs;
using RAGNET.Domain.SharedKernel.Plans.Policies;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Workflows.Commands.EnqueueEmbeddingJob
{
    public class EnqueueEmbeddingJobCommandHandler(
        SubscriptionPolicy policy,
        IEmbeddingJobQueue queue,
        IUserRepository userRepository
    ) : ICommandHandler<EnqueueEmbeddingJobCommand, Guid>
    {
        private readonly SubscriptionPolicy _policy = policy;
        private readonly IEmbeddingJobQueue _queue = queue;
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Guid> Handle(EnqueueEmbeddingJobCommand request, CancellationToken ct)
        {
            if (!request.Workflow.IsActive)
                throw new Exception("Workflow is not active.");

            var user = await _userRepository.GetByIdAsync(request.Workflow.UserId) ?? throw new Exception("Invalid user");
            var size = request.File.Length;

            if (!_policy.AllowsFileSize(user, size))
                throw new Exception("Your plan does not allow files of this size.");

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms, ct);

            var job = new EmbeddingJob
            {
                ApiKey = request.Workflow.ApiKey,
                UserId = user.Id,
                FileName = request.File.FileName,
                FileContent = ms.ToArray(),
                CallbackUrls = request.Workflow.CallbackUrls.Select(x => x.Url).ToList().ToUrlList()
            };

            await _queue.EnqueueAsync(job, ct);

            return job.JobId;
        }
    }

}