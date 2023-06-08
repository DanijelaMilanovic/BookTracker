import React from 'react';
import { render } from '@testing-library/react';
import MyComponent from '../Component/MyComponent';

test('renders component with correct name', () => {
  const name = 'John';
  const { getByText } = render(<MyComponent name={name} />);
  const componentElement = getByText(`Hello, ${name}!`);
  expect(componentElement).toBeInTheDocument(); 
});
