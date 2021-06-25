using Git.ViewModels.Commits;
using Git.ViewModels.Repositories;
using Git.ViewModels.Users;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserViewModel model);

        ICollection<string> ValidateRepository(CreateRepositoryViewModel model);

        ICollection<string> ValidateCommit(CreateCommitViewModel model);
    }
}