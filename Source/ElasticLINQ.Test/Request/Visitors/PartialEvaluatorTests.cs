﻿// Copyright (c) Tier 3 Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 

using ElasticLinq;
using ElasticLinq.Request.Visitors;
using ElasticLINQ.Test.TestSupport;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ElasticLINQ.Test.Request.Visitors
{
    public class PartialEvaluatorTests
    {
        private readonly FakeQuery<Sample> sampleQuery = new FakeQuery<Sample>(new FakeQueryProvider());

        [Fact]
        public void ShouldNotEvaluateParameters()
        {
            var result = PartialEvaluator.ShouldEvaluate(Expression.Parameter(typeof(PartialEvaluatorTests)));

            Assert.False(result);
        }

        [Fact]
        public void ShouldNotEvaluateLambdas()
        {
            Expression<Func<double>> getScoreField = () => ElasticFields.Score;
            var result = PartialEvaluator.ShouldEvaluate(Expression.Lambda(getScoreField));

            Assert.False(result);
        }

        [Fact]
        public void ShouldNotEvaluateElasticFieldProperties()
        {
            Expression<Func<double>> getScoreField = () => ElasticFields.Score;
            var result = PartialEvaluator.ShouldEvaluate(getScoreField);

            Assert.False(result);
        }

        [Fact]
        public void ShouldNotEvaluateQueryableExtensionMethods()
        {
            var result = PartialEvaluator.ShouldEvaluate(sampleQuery.Skip(0).Expression);

            Assert.False(result);
        }

        [Fact]
        public void ShouldNotEvaluateElasticQueryableMethods()
        {
            var result = PartialEvaluator.ShouldEvaluate(sampleQuery.OrderByScore().Expression);

            Assert.False(result);
        }

        private class Sample { }
    }
}