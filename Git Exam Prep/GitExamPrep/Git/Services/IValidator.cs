using Git.ViewModels;
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
