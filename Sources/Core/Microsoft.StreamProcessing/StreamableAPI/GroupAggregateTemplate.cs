﻿// *********************************************************************
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License
// *********************************************************************

using System;
using System.Linq.Expressions;
using Microsoft.StreamProcessing.Aggregates;
using Microsoft.StreamProcessing.Internal;

namespace Microsoft.StreamProcessing
{
    public static partial class Streamable
    {
        /// <summary>
        /// Groups input events by a key selector and applies an aggregate to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(merger, nameof(merger));

            return source.Map(keySelector).Reduce(s => s.Aggregate(aggregate1), merger);
        }

        /// <summary>
        /// Groups input events by a key selector and applies an aggregate to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(merger, nameof(merger));

            return source.Map(keySelector).Reduce(s => s.Aggregate(aggregate1), merger);
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, StructTuple<TOutput1, TOutput2>>> aggregateMerger =
                (output1, output2) => new StructTuple<TOutput1, TOutput2> {
                    Item1 = output1,
                    Item2 = output2
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, StructTuple<TOutput1, TOutput2>>> aggregateMerger =
                (output1, output2) => new StructTuple<TOutput1, TOutput2> {
                    Item1 = output1,
                    Item2 = output2
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, StructTuple<TOutput1, TOutput2, TOutput3>>> aggregateMerger =
                (output1, output2, output3) => new StructTuple<TOutput1, TOutput2, TOutput3> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, StructTuple<TOutput1, TOutput2, TOutput3>>> aggregateMerger =
                (output1, output2, output3) => new StructTuple<TOutput1, TOutput2, TOutput3> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4>>> aggregateMerger =
                (output1, output2, output3, output4) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4>>> aggregateMerger =
                (output1, output2, output3, output4) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5>>> aggregateMerger =
                (output1, output2, output3, output4, output5) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5>>> aggregateMerger =
                (output1, output2, output3, output4, output5) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState11, TOutput11>> aggregate11,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            IAggregate<TInput, TState11, TOutput11> aggregate11,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState11, TOutput11>> aggregate11,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState12, TOutput12>> aggregate12,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            IAggregate<TInput, TState11, TOutput11> aggregate11,
            IAggregate<TInput, TState12, TOutput12> aggregate12,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState11, TOutput11>> aggregate11,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState12, TOutput12>> aggregate12,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState13, TOutput13>> aggregate13,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            IAggregate<TInput, TState11, TOutput11> aggregate11,
            IAggregate<TInput, TState12, TOutput12> aggregate12,
            IAggregate<TInput, TState13, TOutput13> aggregate13,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TState14, TOutput14, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState11, TOutput11>> aggregate11,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState12, TOutput12>> aggregate12,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState13, TOutput13>> aggregate13,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState14, TOutput14>> aggregate14,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(aggregate14, nameof(aggregate14));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13, output14) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13,
                    Item14 = output14
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13, outputs.Item14);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregate14, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TState14, TOutput14, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            IAggregate<TInput, TState11, TOutput11> aggregate11,
            IAggregate<TInput, TState12, TOutput12> aggregate12,
            IAggregate<TInput, TState13, TOutput13> aggregate13,
            IAggregate<TInput, TState14, TOutput14> aggregate14,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(aggregate14, nameof(aggregate14));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13, output14) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13,
                    Item14 = output14
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13, outputs.Item14);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregate14, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        public static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TState14, TOutput14, TState15, TOutput15, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState1, TOutput1>> aggregate1,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState2, TOutput2>> aggregate2,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState3, TOutput3>> aggregate3,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState4, TOutput4>> aggregate4,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState5, TOutput5>> aggregate5,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState6, TOutput6>> aggregate6,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState7, TOutput7>> aggregate7,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState8, TOutput8>> aggregate8,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState9, TOutput9>> aggregate9,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState10, TOutput10>> aggregate10,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState11, TOutput11>> aggregate11,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState12, TOutput12>> aggregate12,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState13, TOutput13>> aggregate13,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState14, TOutput14>> aggregate14,
            Func<Window<CompoundGroupKey<TOuterKey, TInnerKey>, TInput>, IAggregate<TInput, TState15, TOutput15>> aggregate15,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(aggregate14, nameof(aggregate14));
            Invariant.IsNotNull(aggregate15, nameof(aggregate15));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13, output14, output15) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13,
                    Item14 = output14,
                    Item15 = output15
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13, outputs.Item14, outputs.Item15);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregate14, aggregate15, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }

        /// <summary>
        /// Groups input events by a key selector and applies multiple aggregates to "snapshot windows" (SI terminology) on each group.
        /// </summary>
        internal static IStreamable<TOuterKey, TOutput> GroupAggregate<TOuterKey, TInput, TInnerKey, TState1, TOutput1, TState2, TOutput2, TState3, TOutput3, TState4, TOutput4, TState5, TOutput5, TState6, TOutput6, TState7, TOutput7, TState8, TOutput8, TState9, TOutput9, TState10, TOutput10, TState11, TOutput11, TState12, TOutput12, TState13, TOutput13, TState14, TOutput14, TState15, TOutput15, TOutput>(
            this IStreamable<TOuterKey, TInput> source,
            Expression<Func<TInput, TInnerKey>> keySelector,
            IAggregate<TInput, TState1, TOutput1> aggregate1,
            IAggregate<TInput, TState2, TOutput2> aggregate2,
            IAggregate<TInput, TState3, TOutput3> aggregate3,
            IAggregate<TInput, TState4, TOutput4> aggregate4,
            IAggregate<TInput, TState5, TOutput5> aggregate5,
            IAggregate<TInput, TState6, TOutput6> aggregate6,
            IAggregate<TInput, TState7, TOutput7> aggregate7,
            IAggregate<TInput, TState8, TOutput8> aggregate8,
            IAggregate<TInput, TState9, TOutput9> aggregate9,
            IAggregate<TInput, TState10, TOutput10> aggregate10,
            IAggregate<TInput, TState11, TOutput11> aggregate11,
            IAggregate<TInput, TState12, TOutput12> aggregate12,
            IAggregate<TInput, TState13, TOutput13> aggregate13,
            IAggregate<TInput, TState14, TOutput14> aggregate14,
            IAggregate<TInput, TState15, TOutput15> aggregate15,
            Expression<Func<GroupSelectorInput<TInnerKey>, TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15, TOutput>> merger)
        {
            Invariant.IsNotNull(source, nameof(source));
            Invariant.IsNotNull(keySelector, nameof(keySelector));
            Invariant.IsNotNull(aggregate1, nameof(aggregate1));
            Invariant.IsNotNull(aggregate2, nameof(aggregate2));
            Invariant.IsNotNull(aggregate3, nameof(aggregate3));
            Invariant.IsNotNull(aggregate4, nameof(aggregate4));
            Invariant.IsNotNull(aggregate5, nameof(aggregate5));
            Invariant.IsNotNull(aggregate6, nameof(aggregate6));
            Invariant.IsNotNull(aggregate7, nameof(aggregate7));
            Invariant.IsNotNull(aggregate8, nameof(aggregate8));
            Invariant.IsNotNull(aggregate9, nameof(aggregate9));
            Invariant.IsNotNull(aggregate10, nameof(aggregate10));
            Invariant.IsNotNull(aggregate11, nameof(aggregate11));
            Invariant.IsNotNull(aggregate12, nameof(aggregate12));
            Invariant.IsNotNull(aggregate13, nameof(aggregate13));
            Invariant.IsNotNull(aggregate14, nameof(aggregate14));
            Invariant.IsNotNull(aggregate15, nameof(aggregate15));
            Invariant.IsNotNull(merger, nameof(merger));

            Expression<Func<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15>>> aggregateMerger =
                (output1, output2, output3, output4, output5, output6, output7, output8, output9, output10, output11, output12, output13, output14, output15) => new StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15> {
                    Item1 = output1,
                    Item2 = output2,
                    Item3 = output3,
                    Item4 = output4,
                    Item5 = output5,
                    Item6 = output6,
                    Item7 = output7,
                    Item8 = output8,
                    Item9 = output9,
                    Item10 = output10,
                    Item11 = output11,
                    Item12 = output12,
                    Item13 = output13,
                    Item14 = output14,
                    Item15 = output15
                };
            Expression<Func<GroupSelectorInput<TInnerKey>, StructTuple<TOutput1, TOutput2, TOutput3, TOutput4, TOutput5, TOutput6, TOutput7, TOutput8, TOutput9, TOutput10, TOutput11, TOutput12, TOutput13, TOutput14, TOutput15>, TOutput>> reducerTemplate =
                (key, outputs) => CallInliner.Call(merger, key, outputs.Item1, outputs.Item2, outputs.Item3, outputs.Item4, outputs.Item5, outputs.Item6, outputs.Item7, outputs.Item8, outputs.Item9, outputs.Item10, outputs.Item11, outputs.Item12, outputs.Item13, outputs.Item14, outputs.Item15);

            return source.Map(keySelector)
                         .Reduce(
                             s => s.Aggregate(aggregate1, aggregate2, aggregate3, aggregate4, aggregate5, aggregate6, aggregate7, aggregate8, aggregate9, aggregate10, aggregate11, aggregate12, aggregate13, aggregate14, aggregate15, aggregateMerger),
                             reducerTemplate.InlineCalls());
        }
    }
}