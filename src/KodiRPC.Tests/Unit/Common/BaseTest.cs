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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KodiRPC.ExceptionHandling.RPC;
using KodiRPC.Responses.Files;
using KodiRPC.Responses.Types.List;
using KodiRPC.Responses.Types.Media;
using KodiRPC.Responses.Types.Video;
using KodiRPC.RPC.RequestResponse;
using KodiRPC.RPC.RequestResponse.Params;
using Newtonsoft.Json;
using NUnit.Framework;
using File = System.IO.File;

namespace KodiRPC.Tests.Unit.Common
{
    public class BaseTest
    {
        private static readonly List<string> ResponseTypes = new List<string>
        {
            "Cast",
            "Video",
            "Audio",
            "Subtitle",
            "Episode",
            "TvShow",
            "Movie",
            "Season",
            "Details",
            "File",
            "PrepareDownloadResponse"
        };

        public JsonRpcResponse<T> MakeFauxRequest<T>(string file)
        {
            string json;

            try
            {
                json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"/../../App_Data/" + file);
            }
            catch (Exception)
            {
                json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"/../../App_Data/error.json");
            }

            var response = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(json);

            if (response.Error == null)
            {
                return response;
            }

            var internalServerErrorException = new RpcInternalServerErrorException(response.Error.Message)
            {
                RpcErrorCode = response.Error.Code
            };

            throw internalServerErrorException;
        }

        public static void AssertThatPropertyValuesAreEquals(object actual, object expected)
        {
            var properties = expected.GetType().GetProperties();

            foreach (var property in properties)
            {
                var expectedValue = property.GetValue(expected, null);
                var actualValue = property.GetValue(actual, null);

                if (actualValue is IList)
                {
                    AssertThatListsAreEquals(property, (IList)actualValue, (IList)expectedValue, expected.GetType().ToString());
                }
                else if (actualValue is Streams || actualValue is Resume || actualValue is Artwork || actualValue is UniqueId || 
                    actualValue is LimitsReturned || actualValue is Limits || actualValue is Responses.Types.List.Item.File || actualValue is Details)
                {
                    AssertThatPropertyValuesAreEquals(actualValue, expectedValue);
                }
                else if (!Equals(expectedValue, actualValue))
                {
                    Assert.Fail("Property {0}.{1} does not match. Expected: {2} but was: {3}",
                        property.DeclaringType?.Name, property.Name, expectedValue, actualValue);
                }
            }
        }

        private static void AssertThatListsAreEquals(PropertyInfo property, IList actualList, IList expectedList, string parentType)
        {
            if (actualList?.Count != expectedList?.Count)
            {
                Assert.Fail("Property {0}.{1} does not match. Expected IList containing {2} elements but was IList containing {3} elements\nParent Type: {4}", 
                    property.PropertyType.Name, property.Name, expectedList?.Count, actualList?.Count, parentType);
            }

            for (var i = 0; i < actualList?.Count; i++)
            {
                if (ResponseTypes.Contains(actualList[i].GetType().Name))
                {
                    AssertThatPropertyValuesAreEquals(actualList[i], expectedList[i]);
                }
                else if  (!Equals(actualList[i], expectedList[i]))
                {
                    Assert.Fail(
                        "Property {0}.{1} does not match. Expected IList with element {1} equals to {2} but was IList with element {1} equals to {3}",
                        property.PropertyType.Name, property.Name, expectedList[i], actualList[i]);
                }
            }
        }
    }
}