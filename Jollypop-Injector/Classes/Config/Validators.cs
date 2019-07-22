using Registrar;

namespace Jollypop_Injector.ConfigValidators
{
    public class Validators
    {
        class BitnessValidatorClass : IValidator
        {
            public string Description()
            {
                return "Must be 0(32 bit) or 1(64 bit).";
            }

            public bool Validate(object value)
            {
                int convertedValue = ValidatorConverters.ValidatorIntConverter(value);
                return (convertedValue == 0 || convertedValue == 1);
            }
        }

        public IValidator BitnessValidator = new BitnessValidatorClass();
    }
}
