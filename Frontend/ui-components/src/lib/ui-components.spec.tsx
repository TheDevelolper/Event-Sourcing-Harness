import { render } from '@testing-library/react';

import FrontendUiComponents from './ui-components';

describe('FrontendUiComponents', () => {
  it('should render successfully', () => {
    const { baseElement } = render(<FrontendUiComponents />);
    expect(baseElement).toBeTruthy();
  });
});
