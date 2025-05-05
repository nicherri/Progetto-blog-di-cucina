// src/app/core/utils/apply-server-errors.ts
import { FormGroup } from '@angular/forms';

export function applyServerErrors(
  form: FormGroup,
  errors?: Record<string, string[]>,
) {
  if (!errors) return;
  Object.entries(errors).forEach(([field, msgs]) => {
    const c = form.get(field);
    if (c) c.setErrors({ server: msgs.join(' ') });
  });
}
