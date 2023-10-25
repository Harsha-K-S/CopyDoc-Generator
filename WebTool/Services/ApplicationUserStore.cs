using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebTool
{
    public class ApplicationUserStore :
        IUserPasswordStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserStore<ApplicationUser>
    {
        private readonly string _filePath;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        private List<ApplicationUser> _usersCache;
        private List<ApplicationUser> UsersCache => _usersCache ??= ReadUsersFromJson();

        public ApplicationUserStore(string filePath, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _filePath = filePath;
            _passwordHasher = passwordHasher;
        }

        private List<ApplicationUser> ReadUsersFromJson()
        {
            List<ApplicationUser> users = null;
            if (File.Exists(_filePath))
            { 
                string json = File.ReadAllText(_filePath);
                users = JsonSerializer.Deserialize<List<ApplicationUser>>(json);
            }

            return users ?? new List<ApplicationUser>();
        }

        private void SaveUsersToJson()
        {
            string json = JsonSerializer.Serialize(_usersCache, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }

        public void Dispose() { }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ApplicationUser found = FindUserByEmail(user.Email);

            return found != null
                ? Task.FromResult(found.UserName)
                : Task.FromResult((string)null);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ApplicationUser found = FindUserByEmail(user.Email);

            return found != null
                ? Task.FromResult(found.UserName)
                : Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            ApplicationUser existing = FindUserByEmail(user.Email);
            if (existing != null)
            {
                user.UserName = userName;
                existing.UserName = userName;
                SaveUsersToJson();
            }

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ApplicationUser existing = FindUserByEmail(user.Email);
            if (existing != null)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));
            }

            user.NormalizedUserName = user.Email.ToUpperInvariant();
            UsersCache.Add(user);
            SaveUsersToJson();

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ApplicationUser existing = FindUserById(user.Id);
            existing.Name = user.Name;
            existing.UserName = user.UserName;
            existing.Email = user.Email;
            existing.Password = user.Password.IsDefined() ? user.Password : existing.Password;
            existing.Role = user.Role;
            SaveUsersToJson();

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ApplicationUser found = FindUserById(user.Id);
            if (found == null)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "User not found" }));
            }

            UsersCache.Remove(found);
            SaveUsersToJson();
            
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ApplicationUser found = FindUserById(userId);

            return found != null
                ? Task.FromResult(found)
                : Task.FromResult<ApplicationUser>(null);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            ApplicationUser found = FindUserByEmail(normalizedUserName);

            return Task.FromResult(found);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_passwordHasher.HashPassword(user, user.Password));
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.FromResult<ApplicationUser>(null);
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<string>>(new List<string> { user.Role });
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            List<ApplicationUser> users = UsersCache.Where(u => roleName == "*" || u.Role.IgEquals(roleName)).ToList();

            return Task.FromResult<IList<ApplicationUser>>(users);
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            ApplicationUser user = FindUserByEmail(normalizedEmail);

            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private ApplicationUser FindUserByEmail(string email)
        {
            return UsersCache.SingleOrDefault(u => u.Email.IgEquals(email));
        }

        private ApplicationUser FindUserById(string userId)
        {
            return UsersCache.SingleOrDefault(u => u.Id == userId);
        }
    }
}