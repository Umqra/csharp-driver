// 
//       Copyright (C) 2019 DataStax Inc.
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//       http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.NoopImpl
{
    internal class EmptyDriverTimer : IDriverTimer
    {
        public static EmptyDriverTimer Instance = new EmptyDriverTimer();

        public IDriverTimeHandler StartRecording()
        {
            return EmptyDriverTimeHandler.Instance;
        }
    }
}