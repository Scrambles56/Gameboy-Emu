using System.Numerics;

namespace GameboyEmu.Logic.Cpu;

public abstract class Register<T>
    where T : struct, IUnsignedNumber<T>
{

    protected Register(T value)
    {
        Value = value;
    }
    
    protected Register() : this(T.Zero)
    {
    }

    protected T Value { get; set; }
    
    public T GetValue()
    {
        return Value;
    }
    
    public void SetValue(T value)
    {
        Value = value;
    }
    
    public static Register<T> operator ++(Register<T> register)
    {
        register.Value++;
        return register;
    }

    public static implicit operator T(Register<T> register)
    {
        return register.Value;
    }

    public override string? ToString()
    {
        return Value.ToString();
    }
}

public class Register8 : Register<byte>
{
    public Register8(byte value) : base(value)
    {
    }

    public Register8() : base()
    {
    }
}

public class RegisterFlags : Register8
{
    public RegisterFlags(byte value) : base(value)
    {
    }

    public RegisterFlags()
    {
    }

    private bool IsBitSet(int bitNumber) => (Value & (1 << bitNumber - 1)) != 0;
    
    private void SetBit(int bitNumber, bool value)
    {
        if (value)
        {
            Value |= (byte)(1 << bitNumber - 1);
        }
        else
        {
            Value &= (byte)~(1 << bitNumber - 1);
        }
    }
    
    public bool ZeroFlag { 
        get => IsBitSet(7);
        set => SetBit(7, value);
    }

    public bool SubtractFlag
    {
        get => IsBitSet(6);
        set => SetBit(6, value);
    }

    public bool HalfCarryFlag
    {
        get => IsBitSet(5);
        set => SetBit(5, value);
    }

    public bool CarryFlag
    {
        get => IsBitSet(4);
        set => SetBit(4, value);
    }
}

public class Register16 : Register<ushort>
{
    public Register16(ushort value) : base(value)
    {
    }

    public Register16() : base()
    {
    }
    
    public static implicit operator Register16(ushort value)
    {
        return new(value);
    }
    
    public static Register16 operator ++(Register16 register)
    {
        register.Value++;
        return register;
    }

    public static Register16 operator +(Register16 register, ushort value)
    {
        return new((ushort)(register.Value + value));
    }
    
    public static Register16 operator -(Register16 register, ushort value)
    {
        return new((ushort)(register.Value - value));
    }
}