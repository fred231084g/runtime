// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/******************************************************************************
 * This file is auto-generated from a template file by the GenerateTests.csx  *
 * script in tests\src\JIT\HardwareIntrinsics\X86\Shared. In order to make    *
 * changes, please update the corresponding template and run according to the *
 * directions listed in the file.                                             *
 ******************************************************************************/

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Xunit;

namespace JIT.HardwareIntrinsics.X86._Sse41.X64
{
    public static partial class Program
    {
        [Fact]
        public static void InsertInt64129()
        {
            var test = new InsertScalarTest__InsertInt64129();

            if (test.IsSupported)
            {
                // Validates basic functionality works, using Unsafe.Read
                test.RunBasicScenario();

                if (Sse2.IsSupported)
                {
                    // Validates basic functionality works, using Load
                    test.RunBasicScenario_Load();

                    // Validates basic functionality works, using LoadAligned
                    test.RunBasicScenario_LoadAligned();
                }

                // Validates calling via reflection works, using Unsafe.Read
                test.RunReflectionScenario();

                if (Sse2.IsSupported)
                {
                    // Validates calling via reflection works, using Load
                    test.RunReflectionScenario_Load();

                    // Validates calling via reflection works, using LoadAligned
                    test.RunReflectionScenario_LoadAligned();
                }

                // Validates passing a static member works
                test.RunClsVarScenario();

                // Validates passing a local works, using Unsafe.Read
                test.RunLclVarScenario();

                if (Sse2.IsSupported)
                {
                    // Validates passing a local works, using Load
                    test.RunLclVarScenario_Load();

                    // Validates passing a local works, using LoadAligned
                    test.RunLclVarScenario_LoadAligned();
                }

                // Validates passing the field of a local class works
                test.RunClassLclFldScenario();

                // Validates passing an instance member of a class works
                test.RunClassFldScenario();

                // Validates passing the field of a local struct works
                test.RunStructLclFldScenario();

                // Validates passing an instance member of a struct works
                test.RunStructFldScenario();
            }
            else
            {
                // Validates we throw on unsupported hardware
                test.RunUnsupportedScenario();
            }

            if (!test.Succeeded)
            {
                throw new Exception("One or more scenarios did not complete as expected.");
            }
        }
    }

    public sealed unsafe class InsertScalarTest__InsertInt64129
    {
        private struct TestStruct
        {
            public Vector128<Int64> _fld;
            public Int64 _scalarFldData;

            public static TestStruct Create()
            {
                var testStruct = new TestStruct();

                for (var i = 0; i < Op1ElementCount; i++) { _data[i] = TestLibrary.Generator.GetInt64(); }
                Unsafe.CopyBlockUnaligned(ref Unsafe.As<Vector128<Int64>, byte>(ref testStruct._fld), ref Unsafe.As<Int64, byte>(ref _data[0]), (uint)Unsafe.SizeOf<Vector128<Int64>>());

                testStruct._scalarFldData = (long)2;

                return testStruct;
            }

            public void RunStructFldScenario(InsertScalarTest__InsertInt64129 testClass)
            {
                var result = Sse41.X64.Insert(_fld, _scalarFldData, 129);

                Unsafe.Write(testClass._dataTable.outArrayPtr, result);
                testClass.ValidateResult(_fld, _scalarFldData, testClass._dataTable.outArrayPtr);
            }
        }

        private static readonly int LargestVectorSize = 16;

        private static readonly int Op1ElementCount = Unsafe.SizeOf<Vector128<Int64>>() / sizeof(Int64);
        private static readonly int RetElementCount = Unsafe.SizeOf<Vector128<Int64>>() / sizeof(Int64);

        private static Int64[] _data = new Int64[Op1ElementCount];
        private static Int64 _scalarClsData = (long)2;

        private static Vector128<Int64> _clsVar;

        private Vector128<Int64> _fld;
        private Int64 _scalarFldData = (long)2;

        private SimpleUnaryOpTest__DataTable<Int64, Int64> _dataTable;

        static InsertScalarTest__InsertInt64129()
        {
            for (var i = 0; i < Op1ElementCount; i++) { _data[i] = TestLibrary.Generator.GetInt64(); }
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<Vector128<Int64>, byte>(ref _clsVar), ref Unsafe.As<Int64, byte>(ref _data[0]), (uint)Unsafe.SizeOf<Vector128<Int64>>());
        }

        public InsertScalarTest__InsertInt64129()
        {
            Succeeded = true;

            for (var i = 0; i < Op1ElementCount; i++) { _data[i] = TestLibrary.Generator.GetInt64(); }
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<Vector128<Int64>, byte>(ref _fld), ref Unsafe.As<Int64, byte>(ref _data[0]), (uint)Unsafe.SizeOf<Vector128<Int64>>());

            for (var i = 0; i < Op1ElementCount; i++) { _data[i] = TestLibrary.Generator.GetInt64(); }
            _dataTable = new SimpleUnaryOpTest__DataTable<Int64, Int64>(_data, new Int64[RetElementCount], LargestVectorSize);
        }

        public bool IsSupported => Sse41.X64.IsSupported;

        public bool Succeeded { get; set; }

        public void RunBasicScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunBasicScenario));

            var result = Sse41.X64.Insert(
                Unsafe.Read<Vector128<Int64>>(_dataTable.inArrayPtr),
                (long)2,
                129
            );

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(_dataTable.inArrayPtr, (long)2, _dataTable.outArrayPtr);
        }

        public unsafe void RunBasicScenario_Load()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunBasicScenario_Load));

            Int64 localData = (long)2;
            Int64* ptr = &localData;

            var result = Sse41.X64.Insert(
                Sse2.LoadVector128((Int64*)(_dataTable.inArrayPtr)),
                *ptr,
                129
            );

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(_dataTable.inArrayPtr, *ptr, _dataTable.outArrayPtr);
        }

        public unsafe void RunBasicScenario_LoadAligned()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunBasicScenario_LoadAligned));

            Int64 localData = (long)2;
            Int64* ptr = &localData;

            var result = Sse41.X64.Insert(
                Sse2.LoadAlignedVector128((Int64*)(_dataTable.inArrayPtr)),
                *ptr,
                129
            );

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(_dataTable.inArrayPtr, *ptr, _dataTable.outArrayPtr);
        }

        public void RunReflectionScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunReflectionScenario));

            var result = typeof(Sse41.X64).GetMethod(nameof(Sse41.X64.Insert), new Type[] { typeof(Vector128<Int64>), typeof(Int64), typeof(byte) })
                                     .Invoke(null, new object[] {
                                        Unsafe.Read<Vector128<Int64>>(_dataTable.inArrayPtr),
                                        (long)2,
                                        (byte)129
                                     });

            Unsafe.Write(_dataTable.outArrayPtr, (Vector128<Int64>)(result));
            ValidateResult(_dataTable.inArrayPtr, (long)2, _dataTable.outArrayPtr);
        }

        public void RunReflectionScenario_Load()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunReflectionScenario_Load));

            var result = typeof(Sse41.X64).GetMethod(nameof(Sse41.X64.Insert), new Type[] { typeof(Vector128<Int64>), typeof(Int64), typeof(byte) })
                                     .Invoke(null, new object[] {
                                        Sse2.LoadVector128((Int64*)(_dataTable.inArrayPtr)),
                                        (long)2,
                                        (byte)129
                                     });

            Unsafe.Write(_dataTable.outArrayPtr, (Vector128<Int64>)(result));
            ValidateResult(_dataTable.inArrayPtr, (long)2, _dataTable.outArrayPtr);
        }

        public void RunReflectionScenario_LoadAligned()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunReflectionScenario_LoadAligned));

            var result = typeof(Sse41.X64).GetMethod(nameof(Sse41.X64.Insert), new Type[] { typeof(Vector128<Int64>), typeof(Int64), typeof(byte) })
                                     .Invoke(null, new object[] {
                                        Sse2.LoadAlignedVector128((Int64*)(_dataTable.inArrayPtr)),
                                        (long)2,
                                        (byte)129
                                     });

            Unsafe.Write(_dataTable.outArrayPtr, (Vector128<Int64>)(result));
            ValidateResult(_dataTable.inArrayPtr, (long)2, _dataTable.outArrayPtr);
        }

        public void RunClsVarScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunClsVarScenario));

            var result = Sse41.X64.Insert(
                _clsVar,
                _scalarClsData,
                129
            );

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(_clsVar, _scalarClsData,_dataTable.outArrayPtr);
        }

        public void RunLclVarScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunLclVarScenario));

            Int64 localData = (long)2;

            var firstOp = Unsafe.Read<Vector128<Int64>>(_dataTable.inArrayPtr);
            var result = Sse41.X64.Insert(firstOp, localData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(firstOp, localData, _dataTable.outArrayPtr);
        }

        public void RunLclVarScenario_Load()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunLclVarScenario_Load));

            Int64 localData = (long)2;

            var firstOp = Sse2.LoadVector128((Int64*)(_dataTable.inArrayPtr));
            var result = Sse41.X64.Insert(firstOp, localData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(firstOp, localData, _dataTable.outArrayPtr);
        }

        public void RunLclVarScenario_LoadAligned()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunLclVarScenario_LoadAligned));

            Int64 localData = (long)2;

            var firstOp = Sse2.LoadAlignedVector128((Int64*)(_dataTable.inArrayPtr));
            var result = Sse41.X64.Insert(firstOp, localData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(firstOp, localData, _dataTable.outArrayPtr);
        }

        public void RunClassLclFldScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunClassLclFldScenario));

            var test = new InsertScalarTest__InsertInt64129();
            var result = Sse41.X64.Insert(test._fld, test._scalarFldData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(test._fld, test._scalarFldData, _dataTable.outArrayPtr);
        }

        public void RunClassFldScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunClassFldScenario));

            var result = Sse41.X64.Insert(_fld, _scalarFldData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(_fld, _scalarFldData, _dataTable.outArrayPtr);
        }

        public void RunStructLclFldScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunStructLclFldScenario));

            var test = TestStruct.Create();
            var result = Sse41.X64.Insert(test._fld, test._scalarFldData, 129);

            Unsafe.Write(_dataTable.outArrayPtr, result);
            ValidateResult(test._fld, test._scalarFldData, _dataTable.outArrayPtr);
        }

        public void RunStructFldScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunStructFldScenario));

            var test = TestStruct.Create();
            test.RunStructFldScenario(this);
        }

        public void RunUnsupportedScenario()
        {
            TestLibrary.TestFramework.BeginScenario(nameof(RunUnsupportedScenario));

            bool succeeded = false;

            try
            {
                RunBasicScenario();
            }
            catch (PlatformNotSupportedException)
            {
                succeeded = true;
            }

            if (!succeeded)
            {
                Succeeded = false;
            }
        }

        private void ValidateResult(Vector128<Int64> firstOp, Int64 scalarData, void* result, [CallerMemberName] string method = "")
        {
            Int64[] inArray = new Int64[Op1ElementCount];
            Int64[] outArray = new Int64[RetElementCount];

            Unsafe.WriteUnaligned(ref Unsafe.As<Int64, byte>(ref inArray[0]), firstOp);
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<Int64, byte>(ref outArray[0]), ref Unsafe.AsRef<byte>(result), (uint)Unsafe.SizeOf<Vector128<Int64>>());

            ValidateResult(inArray, scalarData, outArray, method);
        }

        private void ValidateResult(void* firstOp, Int64 scalarData, void* result, [CallerMemberName] string method = "")
        {
            Int64[] inArray = new Int64[Op1ElementCount];
            Int64[] outArray = new Int64[RetElementCount];

            Unsafe.CopyBlockUnaligned(ref Unsafe.As<Int64, byte>(ref inArray[0]), ref Unsafe.AsRef<byte>(firstOp), (uint)Unsafe.SizeOf<Vector128<Int64>>());
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<Int64, byte>(ref outArray[0]), ref Unsafe.AsRef<byte>(result), (uint)Unsafe.SizeOf<Vector128<Int64>>());

            ValidateResult(inArray, scalarData, outArray, method);
        }

        private void ValidateResult(Int64[] firstOp,  Int64 scalarData, Int64[] result, [CallerMemberName] string method = "")
        {
            bool succeeded = true;

            for (var i = 0; i < RetElementCount; i++)
            {
                if ((i == 1 ? result[i] != scalarData : result[i] != firstOp[i]))
                {
                    succeeded = false;
                    break;
                }
            }

            if (!succeeded)
            {
                TestLibrary.TestFramework.LogInformation($"{nameof(Sse41.X64)}.{nameof(Sse41.X64.Insert)}<Int64>(Vector128<Int64><9>): {method} failed:");
                TestLibrary.TestFramework.LogInformation($"  firstOp: ({string.Join(", ", firstOp)})");
                TestLibrary.TestFramework.LogInformation($"   result: ({string.Join(", ", result)})");
                TestLibrary.TestFramework.LogInformation(string.Empty);

                Succeeded = false;
            }
        }
    }
}
