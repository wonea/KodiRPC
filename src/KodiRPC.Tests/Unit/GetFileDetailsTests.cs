﻿/********************************************************************************************************************************************
 * Copyright (C) 2016 Pieter-Uys Fourie
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as 
 * published by the Free Software Foundation, either version 3 of the License, or any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see 
 * http://www.gnu.org/licenses/.
 */

using KodiRPC.Responses.Files;
using KodiRPC.RPC.RequestResponse;
using KodiRPC.RPC.RequestResponse.Params.Files;
using KodiRPC.RPC.Specifications.Properties;
using KodiRPC.Tests.Unit.Common;
using KodiRPC.Tests.Unit.Common.ExpectedResults;
using NUnit.Framework;

namespace KodiRPC.Tests.Unit
{
    public partial class GetFileDetailsTests : BaseTest
    {
        [Test]
        public void GivenAString_WhenGettingFileDetails_ItShouldReturnFileDetails()
        {
            var parameters = new GetFileDetailsParams
            {
                File = "1",
                Properties = FileProperties.All()
            };

            var mock = GetKodiServiceMock(parameters);
            var service = mock.Object;
            var actual = service.GetFileDetails(parameters, "UnitTests");
            var expected = Files.GetFileDetails();

            Assert.IsInstanceOf<JsonRpcResponse<GetFileDetailsResponse>>(actual);
            AssertThatPropertyValuesAreEquals(actual.Result, expected);
        }
    }
}