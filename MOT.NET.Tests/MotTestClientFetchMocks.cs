using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace MOT.NET {
    public static class MotTestClientFetchMocks {
        private static Mock<HttpMessageHandler> Setup(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> lambda) {
            Mock<HttpMessageHandler> mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(lambda);
            return mock;
        }

        public static Mock<HttpMessageHandler> SetupAuthenticationMock(string correct) {
            return Setup(
                (m, c) => {
                    if(m.Headers.GetValues("x-api-key").FirstOrDefault() == correct) {
                        return new HttpResponseMessage() {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("[]")
                        };
                    }else{
                        return new HttpResponseMessage() {
                            StatusCode = HttpStatusCode.Forbidden,
                            Content = new StringContent("{\"message\": \"Forbidden\"}")
                        };
                    }
                }
            );
        }

        public static Mock<HttpMessageHandler> SetupRecordResponseMock(string record) {
            return Setup(
                (m, c) => {
                    return new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(record)
                    };
                }
            );
        }

        public static Mock<HttpMessageHandler> SetupStatusCodeMock(HttpStatusCode code) {
            return Setup(
                (m, c) => {
                    return new HttpResponseMessage() {
                        StatusCode = code
                    };
                }
            );
        }
    }
}