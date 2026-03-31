'use client';

import { Layout } from 'antd';

const { Footer: AntFooter } = Layout;

export default function Footer() {
  return (
    <AntFooter
      style={{
        textAlign: 'center',
        background: '#fafafa',
        borderTop: '1px solid #f0f0f0',
        padding: '24px 50px',
      }}
    >
      Hobby ©{new Date().getFullYear()} - Ecommerce Platform
    </AntFooter>
  );
}
