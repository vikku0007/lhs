using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LHSAPI.ApiKeyValidation
{
    public class ApiKeyHandler
    {
        public class ApiKeyRequirement : IAuthorizationRequirement
        {
            public IReadOnlyList<string> ApiKeys { get; set; }


        }

        public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
        {
            public const string API_KEY_HEADER_NAME = "X-API-KEY";

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
            {
                SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
                return Task.CompletedTask;
            }

           

            //public ApiKeyRequirementHandler(IMongoDBRepository<ProjectCollection> projectRepository)
            //{
            //    _projectRepository = projectRepository;
            //}

            private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
            {
                if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
                {

                    var apiKey = authorizationFilterContext.HttpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
                    //Expression<Func<ProjectCollection, bool>> whereCondition = x => (x.ClientSecretKey == apiKey && x.IsActive == true);
                    //var res = _projectRepository.GetById(whereCondition).AsQueryable().FirstOrDefault();

                    if (apiKey != null)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();

                    }


                }
            }
        }
    }
}
