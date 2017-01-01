#region

using System;
using NSubstitute;
using Xunit;

#endregion

namespace Dbc.Tests
{
    public class CheckShould 
    {

        [Fact]
        public void be_able_to_use_exception_factory_when_set()
        {
            //// Arrange
            var factory = Substitute.For<Check.CreateException>();
            factory(Arg.Any<Type>()).Returns(new Exception());

            Check.ExceptionFactory = factory;

            //// Act
            Assert.Throws<Exception>(() => Check.Require<Exception>(false));
            Check.ResetExceptionFactory();

            //// Assert
            factory.Received()(Arg.Any<Type>());
        }

        [Fact]
        public void be_able_to_reset_exception_factory()
        {
            var factory = Substitute.For<Check.CreateException>();
            factory(Arg.Any<Type>()).Returns(new Exception());

            Check.ExceptionFactory = factory;
            Check.ResetExceptionFactory();

            //// Act
            Assert.Throws<Exception>(() => Check.Require<Exception>(false));

            //// Assert
            factory.DidNotReceive()(Arg.Any<Type>());
        }

        #region Require

        [Fact]
        public void throw_exception_with_parameters_when_require_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Require<ArgumentNullException>(false, "test", "test"));
        }

        [Fact]
        public void throw_exception_with_parameters_when_require_is_called_with_lamda_parameters_and_assertion_is_false()
        {

            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Require<ArgumentNullException>(false, c => c("test", "test")));
        }

        [Fact]
        public void throw_exception_with_parameters_when_require_is_called_with_lamda_exception_and_assertion_is_false()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Assert.Throws<ArgumentNullException>(() => Check.Require(false, () => new ArgumentNullException("test", "test")));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void throw_precondition_exception_when_require_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<PreconditionException>(() => Check.Require(false, "test"));
        }

        [Fact]
        public void throw_precondition_exception_when_require_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<PreconditionException>(() => Check.Require(false, ()=> "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_require_is_called_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Require<ArgumentNullException>(true, "test", "test");
        }

        [Fact]
        public void do_not_throw_exception_when_require_is_called_with_lamda_exception_and_assertion_is_true()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Check.Require(true, () => new ArgumentNullException("test", "test"));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void do_not_throw_exception_when_require_is_called_with_lamda_parameters_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Require<ArgumentNullException>(true, c => c("test", "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_require_is_called_and_assertion_is_true()
        {
            //// Assert
            Check.Require(true, "test");
        }

        [Fact]
        public void do_not_throw_exception_when_require_is_called_with_lamda_parameters_and_assertion_is_true()
        {
            //// Assert
            Check.Require(true, () => "test");
        }

        #endregion

        #region Ensure

        [Fact]
        public void throw_exception_with_parameters_when_ensure_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Ensure<ArgumentNullException>(false, "test", "test"));
        }

        [Fact]
        public void throw_exception_with_parameters_when_ensure_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Ensure<ArgumentNullException>(false, c => c("test", "test")));
        }

        [Fact]
        public void throw_exception_with_parameters_when_ensure_is_called_with_lamda_exception_and_assertion_is_false()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Assert.Throws<ArgumentNullException>(() => Check.Ensure(false, () => new ArgumentNullException("test", "test")));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void throw_postcondition_exception_when_ensure_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<PostconditionException>(() =>  Check.Ensure(false, "test"));
        }

        [Fact]
        public void throw_postcondition_exception_when_ensure_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<PostconditionException>(() => Check.Ensure(false, () => "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_ensure_is_called_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Ensure<ArgumentNullException>(true, "test", "test");
        }

        [Fact]
        public void do_not_throw_exception_when_ensure_is_called_with_lamda_exception_and_assertion_is_true()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Check.Ensure(true, () => new ArgumentNullException("test", "test"));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void do_not_throw_exception_when_ensure_is_called_with_lamda_parameters_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Ensure<ArgumentNullException>(true, c => c("test", "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_ensure_is_called_and_assertion_is_true()
        {
            //// Assert
            Check.Ensure(true, "test");
        }

        [Fact]
        public void do_not_throw_exception_when_ensure_is_called_with_lamda_parameters_and_assertion_is_true()
        {
            //// Assert
            Check.Ensure(true, () => "test");
        }

        #endregion

        #region Invariant

        [Fact]
        public void throw_exception_with_parameters_when_invariant_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Invariant<ArgumentNullException>(false, "test", "test"));
        }

        [Fact]
        public void throw_exception_with_parameters_when_invariant_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Ensure<ArgumentNullException>(false, c => c("test", "test")));
        }

        [Fact]
        public void throw_exception_with_parameters_when_invariant_is_called_with_lamda_exception_and_assertion_is_false()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Assert.Throws<ArgumentNullException>(() => Check.Invariant(false, () => new ArgumentNullException("test", "test")));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void throw_invariant_exception_when_invariant_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<InvariantException>(() => Check.Invariant(false, "test"));
        }

        [Fact]
        public void throw_invariant_exception_when_invariant_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<InvariantException>(() => Check.Invariant(false, () => "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_invariant_is_called_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Invariant<ArgumentNullException>(true, "test", "test");
        }

        [Fact]
        public void do_not_throw_exception_when_invariant_is_called_with_lamda_exception_and_assertion_is_true()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Check.Invariant(true, () => new ArgumentNullException("test", "test"));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void do_not_throw_exception_when_invariant_is_called_with_lamda_parameters_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Invariant<ArgumentNullException>(true, c => c("test", "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_invariant_is_called_and_assertion_is_true()
        {
            //// Assert
            Check.Invariant(true, "test");
        }

        [Fact]
        public void do_not_throw_exception_when_invariant_is_called_with_lamda_parameters_and_assertion_is_true()
        {
            //// Assert
            Check.Invariant(true, () => "test");
        }

        #endregion

        #region Assert

        [Fact]
        public void throw_exception_with_parameters_when_assert_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Assert<ArgumentNullException>(false, "test", "test"));
        }

        [Fact]
        public void throw_exception_with_parameters_when_assert_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<ArgumentNullException>(() => Check.Assert<ArgumentNullException>(false, c => c("test", "test")));
        }

        [Fact]
        public void throw_exception_with_parameters_when_assert_is_called_with_lamda_exception_and_assertion_is_false()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Assert.Throws<ArgumentNullException>(() => Check.Assert(false, () => new ArgumentNullException("test", "test")));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void throw_assertion_exception_when_assert_is_called_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<AssertionException>(() => Check.Assert(false, "test"));
        }

        [Fact]
        public void throw_assertion_exception_when_assert_is_called_with_lamda_parameters_and_assertion_is_false()
        {
            //// Assert
            Assert.Throws<AssertionException>(() => Check.Assert(false, () => "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_assert_is_called_with_argument_null_exception_and_assertion_is_true()
        {
            //// Arrange

            //// Act
            Check.Assert<ArgumentNullException>(true, "test", "test");

            //// Assert
        }

        [Fact]
        public void do_not_throw_exception_when_assert_is_called_with_lamda_exception_and_assertion_is_true()
        {
            //// Assert
            // ReSharper disable NotResolvedInText
            Check.Assert(true, () => new ArgumentNullException("test", "test"));
            // ReSharper restore NotResolvedInText
        }

        [Fact]
        public void do_not_throw_exception_when_assert_is_called_with_lamda_parameters_with_argument_null_exception_and_assertion_is_true()
        {
            //// Assert
            Check.Assert<ArgumentNullException>(true, c => c("test", "test"));
        }

        [Fact]
        public void do_not_throw_exception_when_assert_is_called_and_assertion_is_true()
        {
            //// Assert
            Check.Assert(true, "test");
        }

        [Fact]
        public void do_not_throw_exception_when_assert_is_called_with_lamda_parameters_and_assertion_is_true()
        {
            //// Assert
            Check.Assert(true, () => "test");
        }

        #endregion
    }   
}
