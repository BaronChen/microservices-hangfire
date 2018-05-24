import * as React from 'react';
import App from './App';
import {shallow} from 'enzyme';
import { Route } from 'react-router';

import Items from './items/items';
import Login from './login/login';

describe('App tests', () => {
  it('should render three route', () => {
    const wrapper = shallow(<App />);
    expect(wrapper.find(Route).length).toBe(3);
  });

  it('should render route correctly', () => {
    const wrapper = shallow(<App />);
    expect(wrapper.find(Route).get(0).props.path).toBe('/login');
    expect(wrapper.find(Route).get(0).props.component).toBe(Login);
    expect(wrapper.find(Route).get(1).props.path).toBe('/items');
    expect(wrapper.find(Route).get(1).props.component).toBe(Items);
    expect(wrapper.find(Route).get(2).props.path).toBe('/');
    expect(wrapper.find(Route).get(2).props.exact).toBe(true);
  });

})

