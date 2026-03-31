'use client';

import { Layout } from 'antd';
import { Header, Footer } from '@/components/layout';

const { Content } = Layout;

export default function MainLayout({ children }: { children: React.ReactNode }) {
  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Header />
      <Content style={{ padding: '24px 50px', flex: 1 }}>{children}</Content>
      <Footer />
    </Layout>
  );
}
