# Validator
An abstract validator class to esae validation of commands 

## Setup validator sample

```
public class MyCommandValidator : Validator<MyCommand>
{
  protected override void Setup(MyCommand command)
  {
    BreakOnAnyFailure(); //Optional
    AddValidationStep(validationStep1);
    AddValidationStep(validationStep2);
    AddValidationStep(validationStep3).BreakOnFailure(); //Optional
    ...
    AddValidationStep(validationStepn);
  }
  
  private Task<ValidationResult> validationFunc1(MyCommand command)
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

```
public class CreateUserCommand
{
  public property string Name {get; set;}
  public property int YearOfBirth {get; set;}
}

public class CreateUserCommandValidator : Validator<CreateUser>
{
  protected override void Setup(CreateUserCommand command)
  {
    AddValidationStep(NameShouldNotBeEmpty);
    AddValidationStep(NameShouldBeMinimum3Chars);
    AddValidationStep(YearOfBirthShouldBeInThePast)
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

public class Program.cs
{
  var command = new CreateUserCommand()
  {
    Name = "James",
    YearOfBirth = 1925,
  }

  var validator = new CreateUserCommandValidator();
  var result = validator.Validate(command);
}

```

