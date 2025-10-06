// using System.Net;
// using System.Threading.Tasks;
// using TestStack.BDDfy;
// using Xunit;
//
// namespace BankingApp.Tests
// {
//     public class CheckBalanceAuthenticationScenarios
//     {
//         private string _token;
//         private ApiResponse _response;
//
//         [Fact]
//         public void CheckBalance_WithValidAuthToken_ReturnsBalanceAnd200()
//         {
//             this.Given(_ => AUserHasAValidAuthToken())
//                 .When(_ => TheUserCallsBalanceEndpoint())
//                 .Then(_ => TheResponseIsSuccessful())
//                 .And(_ => TheBalanceIsReturned())
//                 .BDDfy();
//         }
//
//         [Fact]
//         public void CheckBalance_WithoutAuthToken_Returns401()
//         {
//             this.Given(_ => AUserDoesNotProvideAnAuthToken())
//                 .When(_ => TheUserCallsBalanceEndpoint())
//                 .Then(_ => TheResponseIsUnauthorized("Authentication required"))
//                 .BDDfy();
//         }
//
//         [Fact]
//         public void CheckBalance_WithInvalidOrExpiredToken_Returns401()
//         {
//             this.Given(_ => AUserProvidesAnInvalidOrExpiredToken())
//                 .When(_ => TheUserCallsBalanceEndpoint())
//                 .Then(_ => TheResponseIsUnauthorized("Token invalid or expired"))
//                 .BDDfy();
//         }
//
//         private void AUserHasAValidAuthToken()
//         {
//             _token = AuthService.GenerateValidToken("user1");
//         }
//
//         private void AUserDoesNotProvideAnAuthToken()
//         {
//             _token = null;
//         }
//
//         private void AUserProvidesAnInvalidOrExpiredToken()
//         {
//             _token = "invalid.or.expired.token";
//         }
//
//         private async Task TheUserCallsBalanceEndpoint()
//         {
//             _response = await ApiClient.GetBalance(_token);
//         }
//
//         private void TheResponseIsSuccessful()
//         {
//             Assert.Equal(HttpStatusCode.OK, _response.StatusCode);
//         }
//
//         private void TheBalanceIsReturned()
//         {
//             Assert.NotNull(_response.Balance);
//         }
//
//         private void TheResponseIsUnauthorized(string expectedMessage)
//         {
//             Assert.Equal(HttpStatusCode.Unauthorized, _response.StatusCode);
//             Assert.Equal(expectedMessage, _response.ErrorMessage);
//         }
//     }
//
//     // Dummy response model just for illustration
//     public class ApiResponse
//     {
//         public HttpStatusCode StatusCode { get; set; }
//         public string Balance { get; set; }
//         public string ErrorMessage { get; set; }
//     }
// }