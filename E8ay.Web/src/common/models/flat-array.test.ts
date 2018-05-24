import { IFlatArray, IFlatArrayItem, getFlatArray} from './flat-array';


interface ITestObject extends IFlatArrayItem {
  name: string
}


describe('FlatArray Test', () => {

  it('Should get empty result when input in null or undefined', () => {
    expect(getFlatArray(null)).toEqual({"byId": {}, "ids": []});
    expect(getFlatArray(undefined)).toEqual({"byId": {}, "ids": []});
  }) 

  it('Should get correct array with input', () => {
    const testInput:ITestObject[] = [1, 2, 3, 4, 5].map( (x:number):ITestObject => ({id: `${x}`, name: `${x}_name` }));

    const result:IFlatArray<ITestObject> = getFlatArray(testInput);

    expect(result.ids.length).toBe(5);

    expect(result.byId['2'].name).toBe('2_name');
    expect(result.byId['5'].name).toBe('5_name');
    expect(result.byId['1'].name).toBe('1_name');
    expect(result.byId['3'].name).toBe('3_name');
    expect(result.byId['4'].name).toBe('4_name');
  })
})
