# Validator
An abstract validator class to esae validation of commands 

## External Validator

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

### In Class Validator
```

public class MyCommand
{
    public string Property1 {get; set;}
    public string Property2 {get; set;}
}

public class MyClass
{

    public string Property1 {get; private set;}
    public string Property2 {get; private set;}

    private MyClass()
    {
    }
    
    public static ValidationResult<MyClass> Create(MyCommand command)
    {
        var instance = new MyClass();

            var validator = new Validator<MyCommand>();
            validator.AddValidationStep(instance.SetProperty1);
            validator.AddValidationStep(instance.SetProperty2);
            (Repeat for each validation step)

            var result = validator.Validate(command);

            return result.Result.ToResultWithValue(instance);
    }

    private ValidationResult SetProperty1(MyCommand command)
    {
        if (command.Property1 is not valid)
        {
            return ValidationResult.Failure(errorMessage);
        }

        Property1 = command.Property1;
        return ValidationResult.Success();
    }

        private ValidationResult SetProperty2(MyCommand command)
    {
        if (command.Property2 is not valid)
        {
            return ValidationResult.Failure(errorMessage);
        }

        Property2 = command.Property2;
        return ValidationResult.Success();
    }
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

// External validator
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


//InternalValidator
public class User
{
    public string Name {get; private set;}
    public int YearOfBirth {get; private set;}

    private User()
    {
    }

    public static ValidationResult<User>Create(CreateUserCommand command)
    {
        var instance = new DummyClassWithValidator();

        var validator = new Validator<CreateUserCommand>();
        validator.AddValidationStep(instance.SetName);
        validator.AddValidationStep(instance.SetYearOfBirth);

        var result = validator.Validate(command);

        return result.Result.ToResultWithValue(instance);
    }

    private ValidationResult SetName(DummyCommand command)
    {
        if (string.IsNullOrEmpty(command.Name))
        {
            return ValidationResult.Failure("Empty name");
        }
        if (command.Name.Length < 3)
        {
            return ValidationResult.Failure("Name is too short");
        }
        Name = command.Name;
        return ValidationResult.Success();
    }

    private ValidationResult SetYearOfBirth(DummyCommand command)
    {
        if (command.YearOfBirth > DateTime.Now.Year - 10)
        {
            return ValidationResult.Failure("You are too young");
        }
        YearOfBirth = command.YearOfBirth;
        return ValidationResult.Success();
    }
}
```

