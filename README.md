
# Validator
An abstract validator class to esae validation of commands 

```csharp
public class MyValidator : Validator<MyCommand>
{
  protected override void Setup(MyCommand command)
  {
    BreakOnAnyFailure();
    AddValidationStep(ValidationStep1);
    AddValidationStep(ValidationStep2);
    AddValidationStep(ValidationStep3).BreakOnFailure();
    ...

    AddValidationStep<ExternalValidationStep1>(constructor params as array);
    AddValidationStep<ExternalValidationStep2>(constructor params as array).BreakOnFailure;

    AddValidationStep(ValidationStepn);
  }

  // async internal validation step  
  private Task<ValidationResult> ValidationStep1(MyCommand command)
  {
    Validation logic here;
    If(validation fails) return ValidationResult.Failure(error message);
    return ValidationResult.Success();
  }

  // non async internal validator
  private ValidationResult ValidationStep2(MyCommand command)
  {
    Validation logic here;
    If(validation fails) return ValidationResult.Failure(error message);
    return ValidationResult.Success();
  }

  (Repeat for each validation step)
}
```


#### Validator Options
* Validator.BreakOnAnyFailure : Validator will stop processing validation steps on first failured validation step found, if any.
* ValidationStep.BreakOnFailure : Validator will stop processing validation steps if this specific validation steps is failured.

## Sample of use

```csharp
public class CreateUserCommand
{
  public property string Name {get; set;}
  public property int YearOfBirth {get; set;}
  public property string CityOfBirth {get; set;}
}


public class CityShouldExistValidationStep : ExternalValidationStep, IExternalValidationStep
{
    public CityShouldExistValidationStep(string cityName)
    {
        CityName = cityName;
    }

    public string CityName {get; private set;}

    public ValidationResult Validate() 
    {
        if(String.IsNullOrEmpty(CityName))
        {
            return ValidationResult.Failure("Invalid city name");
        }

        if (CityName does not exist in DataSource)
        {
            return ValidationResult.Failure("City name does not exist");
        }

        return ValidationResult.Success();
    }
}

public class CreateUserCommandValidator : Validator<CreateUserCommand>
{
  protected override void Setup(CreateUserCommand command)
  {
    AddValidationStep(NameShouldNotBeEmpty);
    AddValidationStep(NameShouldBeMinimum3Chars);
    AddValidationStep(YearOfBirthShouldBeInThePast)
    AddValidationStep<CityShouldExistValidationStep>(command.CityOfBirth)
  }
  
  private Task<ValidationResult> NameShouldNotBeEmpty(CreateUserCommand command)
  {
    if (string.IsNullOrEmpty(command.Name)) return ValidationResult.Failure("Empty name");
    return ValidationResult.Success();
  }

  private Task<ValidationResult> NameShouldBeMinimum3Chars(CreateUserCommand command)
  {
    if (command.Name.Length < 3) return ValidationResult.Failure("Name is too short");
    return ValidationResult.Success();
  }
  
  private Task<ValidationResult> YearOfBirthShouldBeInThePast(CreateUserCommand command)
  {
    if (command.YearOfBirth >= DateTime.Now.Year) return ValidationResult.Failure("Invalid year of birth");
    return ValidationResult.Success();
  }
}

// External validator
public class Program.cs
{
  var command = new CreateUserCommand()
  {
    Name = "James",
    YearOfBirth = 1925,
    CityOfBirth = "Barcelona"
  }

  var validator = new CreateUserCommandValidator();
  var result = validator.Validate(command);
}

```

