import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InputComponent } from './input.component';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('InputComponent', () => {
  let component: InputComponent;
  let fixture: ComponentFixture<InputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, InputComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(InputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render input with correct attributes', () => {
    component.label = 'Test Label';
    component.type = 'number';
    component.name = 'test-input';
    component.min = 1;
    component.max = 10;
    component.step = 0.5;

    fixture.detectChanges();

    const label = fixture.debugElement.query(By.css('label')).nativeElement;
    const input = fixture.debugElement.query(By.css('input')).nativeElement;

    expect(label.textContent).toBe('Test Label');
    expect(input.type).toBe('number');
    expect(input.name).toBe('test-input');
    expect(input.min).toBe('1');
    expect(input.max).toBe('10');
    expect(input.step).toBe('0.5');
  });

  it('should update value on input change', () => {
    const input = fixture.debugElement.query(By.css('input')).nativeElement;

    component.type = 'number';
    fixture.detectChanges();
    input.type = 'number';

    spyOn(component, 'onInputChange').and.callThrough();
    spyOn(component, 'onChange');

    input.value = '42';
    input.dispatchEvent(new Event('input'));

    fixture.detectChanges();

    expect(component.onInputChange).toHaveBeenCalled();
    expect(component.onChange).toHaveBeenCalledWith(42);
    expect(component.value).toBe(42);
  });

  it('should call onTouched on blur', () => {
    spyOn(component, 'onTouched');

    const input = fixture.debugElement.query(By.css('input')).nativeElement;
    input.dispatchEvent(new Event('blur'));

    expect(component.onTouched).toHaveBeenCalled();
  });

  it('should handle writeValue correctly', () => {
    component.writeValue('Test Value');
    fixture.detectChanges();

    expect(component.value).toBe('Test Value');

    const input = fixture.debugElement.query(By.css('input')).nativeElement;
    expect(input.value).toBe('Test Value');
  });

  it('should register and call onChange function', () => {
    const mockFn = jasmine.createSpy('onChangeSpy');
    component.registerOnChange(mockFn);

    const inputElement = document.createElement('input');
    inputElement.value = '123';

    const event = new Event('input', { bubbles: true });
    Object.defineProperty(event, 'target', { value: inputElement });

    component.onInputChange(event);

    expect(mockFn).toHaveBeenCalledWith('123');
  });

  it('should register and call onTouched function', () => {
    const mockFn = jasmine.createSpy('onTouchedSpy');
    component.registerOnTouched(mockFn);

    component.onTouched();

    expect(mockFn).toHaveBeenCalled();
  });

  it('should update value when type is number', () => {
    component.type = 'number';
    fixture.detectChanges();

    const input = fixture.debugElement.query(By.css('input')).nativeElement;
    input.value = '123';
    input.dispatchEvent(new Event('input'));

    expect(component.value).toBe(123);
  });
});
