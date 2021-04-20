using Farmacio_API.Contracts.Requests.Followings;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreatePatientFollowRequestExample : IExamplesProvider<CreatePatientFollowRequest>
    {
        public CreatePatientFollowRequest GetExamples()
        {
            return new CreatePatientFollowRequest
            {
                PatientId = new Guid("08d8fae0-4104-4c69-8b05-54b9bf3acd7b"),
                PharmacyId = new Guid("08d8f514-5790-438f-88f7-09089846f3d2")
            };
        }
    }
}