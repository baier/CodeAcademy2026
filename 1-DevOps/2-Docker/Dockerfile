# stage 1: Install dependencies
# load baseimage
FROM node:20-alpine AS deps

#install required library
RUN apk add --no-cache libc6-compat

# set app as working directory
WORKDIR /app

# activate corepack for yarn v4 support
RUN corepack enable

# copy pacakge files
COPY package.json yarn.lock .yarnrc.yml ./

# install dependencies
RUN yarn install --immutable

# stage 2: Build application
# load baseimage (non-dependent on previous stage)
FROM node:20-alpine AS builder
WORKDIR /app
RUN corepack enable

# Define build-time arguments and expose as env-variables
ARG NEXT_PUBLIC_ENABLE_IDEM_FORM=false
ARG NEXT_PUBLIC_SHOW_SEEDED_IDEMS=false
ENV NEXT_PUBLIC_ENABLE_IDEM_FORM = $NEXT_PUBLIC_ENABLE_IDEM_FORM
ENV NEXT_PUBLIC_SHOW_SEEDED_IDEMS = $NEXT_PUBLIC_SHOW_SEEDED_IDEMS

# copy files from deps-stage
COPY --from=deps /app/node_modules ./node_modules
COPY --from=deps /app/.yarn ./.yarn
COPY . .

RUN yarn build

# stage 3: runner - produksjonsimage
FROM node:20-alpine AS runner
WORKDIR /app
ENV NODE_ENV=production
ENV NEXT_TELEMETRY_DISABLED=1

# create system group and system user for security purposes
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

# copy necessary output from builder-stage
COPY --from=builder /app/public ./public
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# switch to nextjs user
USER nextjs

#expose port number 3000
EXPOSE 3000
ENV PORT=3000
ENV HOSTNAME="0.0.0.0"

CMD ["node", "server.js"]